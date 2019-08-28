using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;

namespace ManjuuCommon.Tools
{
    /// <summary>
    /// Excel操作
    /// </summary>
   public class ExcelOptr
    {
        /// <summary>
        /// 解析excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileStream"></param>
        /// <param name="mappers"></param>
        /// <returns></returns>
        public static List<T> ResolveExcelFile<T>(Stream fileStream , ExcelMapper[] mappers)
            where T:class,new()
        {
            if (null == mappers || mappers.Length == 0)
            {
                return null;
            }

            //获取类型数据
           Type typeData =  typeof(T);
            List<T> list = new List<T>();
            //匹配列
            bool matchCloumn = mappers.Any(p => !p.CellColumn.HasValue);

            using (ExcelPackage package = new ExcelPackage(fileStream))
            {
               
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    //获取当前工作簿的有效行列
                    var  dimension = worksheet.Dimension;
                    if (null == dimension)
                    {
                        //当前工作簿没有内容
                        continue;
                    }

                    //获取这个工作簿的行数跟列数
                    int rowCount = dimension.Rows;
                    int columnCount = dimension.Columns;


                    for (int row = 1; row <= rowCount; row++)
                    {

                        T currentInstance = null;
                        for (int column = 1; column <= columnCount; column++)
                        {
                            var cellVal = worksheet.Cells[row, column].GetValue<string>();

                            #region 匹配列位置
                            if (1 == row && matchCloumn)
                            {
                                ExcelMapper macthMapper = mappers.FirstOrDefault(p => p.Cell == cellVal);
                                if (null != macthMapper) { macthMapper.CellColumn = column; }
                                continue;
                            }
                            #endregion


                            ExcelMapper mapper = mappers.FirstOrDefault(p => (p.CellColumn??0)== column);
                            PropertyInfo propertyInfo = typeData.GetProperty(mapper.Property);
                            if (null == propertyInfo)
                            {
                                continue;
                            }

                            currentInstance = currentInstance??(T)Activator.CreateInstance(typeData);
                            if (null == currentInstance)
                            {
                                continue;
                            }
                            propertyInfo.SetValue(currentInstance, cellVal);
                        }
                        if (null != currentInstance)
                        {
                            list.Add(currentInstance);
                        }
                    }
                }
            }

            return (null == list || !list.Any()) ? null : list;
        }

        public static ExcelPackage CreateExcel(ExcelConfig excelConfig)
        {

            if (excelConfig == null)
            {
                return null;
            }

            var package = new ExcelPackage();

            package.Workbook.Properties.Title = excelConfig.Title;//"Salary Report";
            package.Workbook.Properties.Author = excelConfig.Author; //"Vahid N.";
            package.Workbook.Properties.Subject = excelConfig.Subject;//"Salary Report";
            package.Workbook.Properties.Keywords = excelConfig.Keywork;//"Salary";

            ExcelWorksheet currentWorksheet = null;
            if (null == excelConfig.Sheets || 0 == excelConfig.Sheets.Length)
            {
                currentWorksheet = package.Workbook.Worksheets.Add("Sheet1");
                if (null != excelConfig.CreateWorksheetCallBack)
                {
                    excelConfig.CreateWorksheetCallBack(currentWorksheet);
                }
            }
            else
            {
                foreach (var item in excelConfig.Sheets)
                {
                    currentWorksheet = package.Workbook.Worksheets.Add(item);
                    if (null != excelConfig.CreateWorksheetCallBack)
                    {
                        excelConfig.CreateWorksheetCallBack(currentWorksheet);
                    }
                }
            }


            return package;
        }
    }


    public class ExcelMapper
    {
        /// <summary>
        /// 单元格
        /// </summary>
        public string Cell { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// 属性对应第几列单元格
        /// </summary>
        public int? CellColumn { get; set; }

        public ExcelMapper()
        {

        }

        public ExcelMapper(string cell, string property, int? cellColumn=null)
        {
            Cell = cell;
            Property = property;
            CellColumn = cellColumn;
        }
    }

    public class ExcelConfig
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywork { get; set; } = string.Empty;

        public string[] Sheets { get; set; }

        public Action<ExcelWorksheet> CreateWorksheetCallBack { get; set; }
    }

}
