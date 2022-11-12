using _2C2P.Assessment.BusinessLayer;
using _2C2P.Assessment.DataLayer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace _2C2P.Assessment.API.Controllers
{
    [ApiController]
    [EnableCors("OpenPolicy")]
    public class FileController : ControllerBase
    {
        [Route("api/file/upload")]
        [HttpPost]
        public Result UploadFile(FileInput input)
        {
            var result = new Result() { IsSuccess = false };
            try
            {
                bool success = false;
                result.Message = FileManagement.UploadFile(input, ref success);
                result.IsSuccess = success;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        [Route("api/file/data/saved/bycurrency")]
        [HttpPost]
        public DataResult GetSaveDataByCurrency(List<string>? input)
        {
            var result = new DataResult() { IsSuccess = false };
            try
            {
                result.Data = FileManagement.GetSavedFileData(input);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        [Route("api/file/data/saved/bystatus")]
        [HttpPost]
        public DataResult GetSaveDataByStatus(List<string>? input)
        {
            var result = new DataResult() { IsSuccess = false };
            try
            {
                result.Data = FileManagement.GetSavedFileData(null, input);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        [Route("api/file/data/saved/bydate")]
        [HttpPost]
        public DataResult GetSaveDataByDateRange(DateFilter input)
        {
            var result = new DataResult() { IsSuccess = false };
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                var from = DateTime.ParseExact(input.DateFrom, "yyyy-MM-dd", provider);
                var to = DateTime.ParseExact(input.DateTo, "yyyy-MM-dd", provider);
                if (from > to)
                {
                    result.Message = "Invalid date range.";
                    return result;
                }
                result.Data = FileManagement.GetSavedFileData(null, null, from, to);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

    }
}
