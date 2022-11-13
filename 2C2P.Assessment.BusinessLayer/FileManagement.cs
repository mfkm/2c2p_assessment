using _2C2P.Assessment.DataLayer;
using _2C2P.Assessment.DataLayer.Models;
using CsvHelper;
using CsvHelper.Configuration;
using EFCore.BulkExtensions;
using Newtonsoft.Json;
using System.Globalization;
using System.Xml;

namespace _2C2P.Assessment.BusinessLayer
{
    public class FileManagement
    {
        public static string UploadFile(FileInput input, ref bool success)
        {
            success = false;
            if (input == null)
                return "Invalid file input!";
            if (input.FileExtension != "csv" && input.FileExtension != "xml")
                return "Unknown format!";
            if (input.FileSize > 1024000)
                return "The maximum allowed file size is 1MB.";
            using (var db = new AssessmentDbContext())
            {
                var importedData = new List<ImportedDatum>();
                string filePath = string.Empty;
                string uploadErrMsg = string.Empty;
                bool uploadStatus = SaveFileFromBase64(input.Base64String, input.FileExtension, ref filePath, ref uploadErrMsg);
                if (!uploadStatus)
                    return uploadErrMsg;
                CultureInfo provider = CultureInfo.InvariantCulture;
                int cnt = 1;
                using (var sr = new StreamReader(filePath))
                {
                    if (input.FileExtension == "xml")
                    {
                        var fileContent = sr.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(fileContent);
                        dynamic? data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeXmlNode(doc).Replace("@", string.Empty).Trim());
                        if (data == null)
                            return "Unknown format!";
                        if (data.Transactions == null)
                            return "Unknown format!";
                        if (data.Transactions.Transaction == null)
                            return "Unknown format!";
                        List<XMLTransactionObj> txList = new List<XMLTransactionObj>();
                        try
                        {
                            txList = JsonConvert.DeserializeObject<List<XMLTransactionObj>>(data.Transactions.Transaction.ToString());
                        }
                        catch (Exception)
                        {
                            return "Unknown format!";
                        }
                        foreach (var item in txList)
                        {
                            if (string.IsNullOrEmpty(item.id))
                                return "No Transaction Id found for item #" + cnt + " in the XML file!";
                            if (string.IsNullOrEmpty(item.TransactionDate))
                                return "No Transaction Date found for Transaction Id: " + item.id + " in the XML file!";
                            if (item.PaymentDetails == null)
                                return "No Payment Details found for Transaction Id: " + item.id + " in the XML file!";
                            if (string.IsNullOrEmpty(item.PaymentDetails.Amount))
                                return "No Amount found for Transaction Id: " + item.id + " in the XML file!";
                            if (string.IsNullOrEmpty(item.PaymentDetails.CurrencyCode))
                                return "No Currency Code found for Transaction Id: " + item.id + " in the XML file!";
                            if (string.IsNullOrEmpty(item.Status))
                                return "No Status found for Transaction Id: " + item.id + " in the XML file!";
                            importedData.Add(new ImportedDatum()
                            {
                                TxId = item.id,
                                TxDate = DateTime.ParseExact(item.TransactionDate, "yyyy-MM-ddTHH:mm:ss", provider),
                                Amount = double.Parse(item.PaymentDetails.Amount),
                                CurrencyCode = item.PaymentDetails.CurrencyCode,
                                Status = item.Status,
                                FinalStatus = (item.Status == "Approved") ? "A" : (item.Status == "Done") ? "D" : "R",
                                SourceData = "xml"
                            });
                            cnt++;
                        }
                    }
                    if (input.FileExtension == "csv")
                    {
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
                        using (var csv = new CsvReader(sr, config))
                        {
                            var records = csv.GetRecords<CSVTransactionObj>();
                            foreach (var item in records)
                            {
                                if (string.IsNullOrEmpty(item.TxId))
                                    return "No Transaction Id found at row #" + cnt + " in the CSV file!";
                                if (string.IsNullOrEmpty(item.TxDate))
                                    return "No Transaction Date found for Transaction Id: " + item.TxId + " in the CSV file!";
                                if (string.IsNullOrEmpty(item.Amount))
                                    return "No Amount found for Transaction Id: " + item.TxId + " in the CSV file!";
                                if (string.IsNullOrEmpty(item.CurrencyCode))
                                    return "No Currency Code found for Transaction Id: " + item.TxId + " in the CSV file!";
                                if (string.IsNullOrEmpty(item.Status))
                                    return "No Status found for Transaction Id: " + item.TxId + " in the CSV file!";
                                importedData.Add(new ImportedDatum()
                                {
                                    TxId = item.TxId,
                                    TxDate = DateTime.ParseExact(item.TxDate, "dd/MM/yyyy hh:mm:ss", provider),
                                    Amount = double.Parse(item.Amount),
                                    CurrencyCode = item.CurrencyCode,
                                    Status = item.Status,
                                    FinalStatus = (item.Status == "Approved") ? "A" : (item.Status == "Finished") ? "D" : "R",
                                    SourceData = "csv"
                                });
                                cnt++;
                            }
                        }
                    }
                }
                var newDataIds = importedData.Select(x => x.TxId).ToList();
                var existing = db.ImportedData.Where(p => newDataIds.Contains(p.TxId)).FirstOrDefault();
                if (existing != null)
                    return "Transaction Id : " + existing.TxId + " already exist!";
                db.BulkInsert(importedData);
                db.SaveChanges();
                success = true;
            }
            return "File successfully uploaded!";
        }

        public static bool SaveFileFromBase64(string base64String, string fileExtension, ref string filePath, ref string errMsg)
        {
            try
            {
                fileExtension = !fileExtension.Contains('.') ? ("." + fileExtension) : fileExtension;
                filePath = AppDomain.CurrentDomain.BaseDirectory + "/Uploads/";
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                filePath = filePath + Guid.NewGuid().ToString() + fileExtension;
                File.WriteAllBytes(filePath, Convert.FromBase64String(base64String));
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        public static List<SavedData> GetSavedFileData(List<string>? currencyFilter = null, List<string>? statusFilter = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            using (var db = new AssessmentDbContext())
            {
                var query = db.ImportedData.ToList();
                if (currencyFilter != null)
                    query = query.Where(d => currencyFilter.Contains(d.CurrencyCode)).ToList();
                if (statusFilter != null)
                    query = query.Where(d => statusFilter.Contains(d.FinalStatus)).ToList();
                if (dateFrom != null && dateTo != null)
                    query = query.Where(d => d.TxDate >= dateFrom && d.TxDate <= dateTo).ToList();
                return query.Select(d => new SavedData()
                {
                    id = d.TxId,
                    payment = d.Amount + " " + d.CurrencyCode,
                    status = d.FinalStatus,
                }).ToList();
            }
        }

    }
}
