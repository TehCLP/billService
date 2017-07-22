using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        loadBills();
    }

    private void loadBills()
    {
        string sCID = "1,2";
        string sql = string.Format(
        @"  select c.CID, p.ProjectName, c.CName, c.No, 
                t.MeterNoBefore, Format(t.MeterNoBeforeDate, 'dd/mm/yyyy') as MeterNoBeforeDate,
                t.MeterNo, Format(t.MeterNoDate, 'dd/mm/yyyy') as MeterNoDate, 
                t.Prices, t.MeterRate, t.ServiceRate
            from (tbCustomers c 
                inner join tbProjects p on c.PID = p.PID)
                inner join tbTransactions t on c.CID = t.CID and c.MeterNo = t.MeterNo
            where c.CID in ({0})"
            , sCID);

        Access_Conn conn = new Access_Conn();
        DataTable dt = new DataTable();
        dt = conn.getDataTable(sql);

        ReportDocument rpt = new ReportDocument();
        rpt.Load(Server.MapPath("CrystalReport1.rpt"));
        rpt.SetDataSource(dt);

        string pathPDF = Server.MapPath("bills/") + string.Format("bill_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
        rpt.ExportToDisk(ExportFormatType.PortableDocFormat, pathPDF);

        this.CrystalReportViewer1.ReportSource = rpt;
        this.CrystalReportViewer1.RefreshReport();
    }
}