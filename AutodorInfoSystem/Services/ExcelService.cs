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
                var worksheet = workbook.Worksheets.Add(project.Name);
                GenerateProjectWorksheet(worksheet, project);
                await System.Threading.Tasks.Task.Run(() => workbook.SaveAs(memoryStream));
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        private void GenerateProjectWorksheet(IXLWorksheet worksheet, Project project)
        {
            throw new NotImplementedException();
        }
    }
}
