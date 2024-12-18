using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace BBK.HRMS.Reports
{
    public class CrystalReportToPDF
    {
        private readonly DataTable inputParametersTable;
        private readonly string connectionString;
        private readonly string reportPath;

        public CrystalReportToPDF(string reportPath, DataTable inputParameters, string connectionString)
        {
            this.connectionString = connectionString;
            this.reportPath = reportPath;
            this.inputParametersTable = inputParameters;
        }

        public void CreateReportPDF()
        {
            ReportDocument reportDocument = new ReportDocument();

            try
            {
                reportDocument.Load(reportPath);
                reportDocument.Database.Tables[0].ApplyLogOnInfo(LogonInfo());

                DataTable reportDataSourceTable = GetReportDataTable(inputParametersTable, "PROC5_RPT_SSS_LIST_OF_MONTHLY_DISTRIBUTION");
                DataSet ds = new DataSet();
                ds.Tables.Add(reportDataSourceTable);
                ds.Tables.Add(inputParametersTable);
                reportDocument.SetDataSource(ds);

                using (Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    byte[] bytes = new MemoryStream().ToArray();
                    stream.CopyTo(new MemoryStream(bytes));
                    string base64EncodedString = Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            reportDocument.Dispose();
        }

        private TableLogOnInfo LogonInfo()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            return new TableLogOnInfo
            {
                ConnectionInfo =
                {
                    ServerName = builder.DataSource,
                    DatabaseName = builder.InitialCatalog,
                    UserID = builder.UserID,
                    Password = builder.Password
                }
            };
        }

        private DataTable GetReportDataTable(DataTable inputParameters, string procedureName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (DataRow row in inputParameters.Rows)
                {
                    command.Parameters.AddWithValue(row["ParameterName"].ToString(), row["ParameterValue"]);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable outputDataTable = new DataTable();
                adapter.Fill(outputDataTable);

                return outputDataTable;
            }
        }
    }
}
