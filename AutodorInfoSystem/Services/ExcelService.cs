using AutodorInfoSystem.Models;
using ClosedXML.Excel;

namespace AutodorInfoSystem.Services
{
    public class ExcelService
    {
        public async Task<MemoryStream> GenerateProjectReportAsync(Project project)
        {
            var memoryStream = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add($"Проект {project.IdProject}");
                GenerateProjectWorksheet(worksheet, project);
                await System.Threading.Tasks.Task.Run(() => workbook.SaveAs(memoryStream));
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        private void GenerateProjectWorksheet(IXLWorksheet worksheet, Project project)
        {
            worksheet.Cell(1, 1).Value = "Смета";
            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 6)).Merge();

            worksheet.Cell(2, 1).Value = $"по проекту \"{project.Name}\"";
            worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(2, 1).Style.Alignment.WrapText = true;
            worksheet.Cell(2, 1).Style.Font.Bold = true;
            worksheet.Range(worksheet.Cell(2, 1), worksheet.Cell(2, 6)).Merge();

            worksheet.Cell(3, 1).Value = "Наименование задачи";
            worksheet.Cell(3, 2).Value = "Наименование ресурса";
            worksheet.Cell(3, 3).Value = "Количество";
            worksheet.Cell(3, 4).Value = "Единица измерения";
            worksheet.Cell(3, 5).Value = "Цена, зарплата, руб.";
            worksheet.Cell(3, 6).Value = "Стоимость, руб.";
            worksheet.Range(worksheet.Cell(3, 1), worksheet.Cell(3, 6)).Style.Font.Bold = true;

            int row = 3;
            foreach (var task in project.Tasks)
            {
                row++;
                worksheet.Cell(row, 1).Value = task.Name;
                worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 6)).Merge();
                if (task.MaterialsHasTasks.Count > 0) 
                {
                    row++;
                    worksheet.Cell(row, 2).Value = "Материалы";
                    worksheet.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Range(worksheet.Cell(row, 2), worksheet.Cell(row, 6)).Merge();
                    worksheet.Cell(row, 2).Style.Font.Bold = true;
                    foreach (var material in task.MaterialsHasTasks)
                    {
                        row++;
                        worksheet.Cell(row, 2).Value = material.IdMaterialNavigation.Name;
                        worksheet.Cell(row, 3).Value = material.Quantity;
                        worksheet.Cell(row, 4).Value = material.IdMaterialNavigation.MeasurementUnit;
                        worksheet.Cell(row, 5).Value = material.IdMaterialNavigation.Price;
                        worksheet.Cell(row, 6).Value = material.Cost;
                    } 
                }
                if (task.EquipmentHasTasks.Count > 0)
                {
                    row++;
                    worksheet.Cell(row, 2).Value = "Спецтехника";
                    worksheet.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Range(worksheet.Cell(row, 2), worksheet.Cell(row, 6)).Merge();
                    worksheet.Cell(row, 2).Style.Font.Bold = true;
                    foreach (var equipment in task.EquipmentHasTasks)
                    {
                        row++;
                        worksheet.Cell(row, 2).Value = equipment.IdEquipmentNavigation.Name;
                        worksheet.Cell(row, 3).Value = equipment.Quantity;
                        worksheet.Cell(row, 4).Value = "шт";
                        worksheet.Cell(row, 5).Value = equipment.IdEquipmentNavigation.Price;
                        worksheet.Cell(row, 6).Value = equipment.Cost;
                    }
                }
                if (task.WorkersHasTasks.Count > 0)
                {
                    row++;  
                    worksheet.Cell(row, 2).Value = "Рабочие";
                    worksheet.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Range(worksheet.Cell(row, 2), worksheet.Cell(row, 6)).Merge();
                    worksheet.Cell(row, 2).Style.Font.Bold = true;
                    foreach (var worker in task.WorkersHasTasks)
                    {
                        row++;
                        worksheet.Cell(row, 2).Value = worker.IdWorkerNavigation.Name;
                        worksheet.Cell(row, 3).Value = worker.Quantity;
                        worksheet.Cell(row, 5).Value = worker.IdWorkerNavigation.Salary;
                        worksheet.Cell(row, 6).Value = worker.Cost;
                    }
                }
            }
            row++;
            worksheet.Cell(row, 1).Value = "Всего:";
            worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Range(worksheet.Cell(row, 1), worksheet.Cell(row, 5)).Merge();
            worksheet.Cell(row, 1).Style.Font.Bold = true;
            worksheet.Cell(row, 6).Value = project.Cost;
            worksheet.Cell(row, 6).Style.Font.Bold = true;
            worksheet.Columns().AdjustToContents();
            worksheet.Rows().AdjustToContents();
            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(row, 6)).Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);
            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(row, 6)).Style.Border.SetLeftBorder(XLBorderStyleValues.Medium);
            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(row, 6)).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(row, 6)).Style.Border.SetTopBorder(XLBorderStyleValues.Medium);
        }
    }
}
