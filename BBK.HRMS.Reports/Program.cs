using System;
using System.Data;

namespace BBK.HRMS.Reports
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string reportPath = "C:\\Users\\Equilap41\\Documents\\GitHub\\HRMS\\BBK-HRMS-MVC\\HRMS\\Reports\\M5\\rptM5_ListMnthDistribution.rpt";
            string connectionString = "server=.;Database=BBKDB;User Id=sa;Password=123;TrustServerCertificate=True";
            
            DataTable inputParameters = GetInputParameters();
            try
            {

                CrystalReportToPDF reportToPDF = new CrystalReportToPDF(reportPath, inputParameters, connectionString);
                reportToPDF.CreateReportPDF();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while generating the report:");
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                Console.ReadLine();
            }
        }

      
        private static DataTable GetInputParameters()
        {
            DataTable inputParameters = new DataTable();
            inputParameters.Columns.Add("ParameterName", typeof(string));
            inputParameters.Columns.Add("ParameterValue", typeof(object));

            inputParameters.Rows.Add("@i_Company_Cd", "BBK");
            inputParameters.Rows.Add("@i_Group_Cd", "");
            inputParameters.Rows.Add("@i_Division_Cd", "");
            inputParameters.Rows.Add("@i_Department_Cd", "");
            inputParameters.Rows.Add("@i_Section_Cd", "");
            inputParameters.Rows.Add("@i_Emp_No", "1763");
            inputParameters.Rows.Add("@i_For_Month", "01/01/2021");
            inputParameters.Rows.Add("@i_Incharger_Emp_No", "1763");
            inputParameters.Rows.Add("@i_Status", "A");
            return inputParameters;
        }
    }
}
