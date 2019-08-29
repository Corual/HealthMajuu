using ManjuuCommon.Tools;
using ManjuuDomain.Suppers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuDomain.FileAssist
{
    /// <summary>
    /// excel文件
    /// </summary>
    public class ExcelFile : SupEntity,IDisposable
    {
        /// <summary>
        /// excel的文件流
        /// </summary>
        public Stream FileStream { get; private set; }


        public ExcelFile(Stream fileStream)
        {
            if (null == fileStream)
            {
                throw new ArgumentNullException($"param '{fileStream}' is null");
            }

            FileStream = fileStream;
        }

        public static async Task<ExcelFile> CreateFormFormFileAsync(IFormFile file)
        {
            if (null == file)
            {
                throw new ArgumentNullException($"param '{file}' is null");
            }

            ExcelFile excelFile = null;

            MemoryStream stream = new MemoryStream();

            await file.CopyToAsync(stream);

            excelFile = new ExcelFile(stream);


            return excelFile;

        }


        public List<T> GetExcelData<T>(ExcelMapper[] excelMappers)
             where T : class, new()
        {
            return ExcelOptr.ResolveExcelFile<T>(FileStream, excelMappers);
        }

        public static bool ValidExcelFile(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);

            //var fileExtensions = new string[] { ".xlsx", ".xls", ".csv" };
            //if (!fileExtensions.Contains(extension))
            //{
            //    return new JsonDataMsg<string>(null, false, "您上传的文件不是Excel文件");
            //}

            return ".xlsx" == extension;
        }


        public void Dispose()
        {
            FileStream?.Dispose();
        }
    }
}
