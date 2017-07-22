using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Customers : System.Web.UI.Page
{
    Access_Conn conn = new Access_Conn();
    private const string _ssid = "QryGVListCustomter";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindDDL();

            Session.Remove(_ssid);
            Session[_ssid] = "select p.PID,ProjectName,CID,CName,[No],MeterNo,MeterDate,p.MeterRate,p.ServiceRate from tbProjects p inner join tbCustomers c on p.PID = c.PID where p.PID = {0}";

            clear();

            try { bindData(string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue), gvListProject); }
            catch (Exception ex) { string sssss = ex.ToString(); Response.Write(sssss); }
        }
    }

    private void bindDDL()
    {
        string sql = "select PID,ProjectName from tbProjects";
        string txt = "ProjectName";
        string val = "PID";
        conn.getDataToDDL(ref ddlProjectName, sql, txt, val);
    }
    private void bindData(string sql, GridView gv)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        dt = conn.getDataTable(sql + " order by c.CID");

        if (dt != null && dt.Rows.Count > 0)
        {
            hdfMR.Value = dt.Rows[0]["MeterRate"].ToString();
            hdfSR.Value = dt.Rows[0]["ServiceRate"].ToString();
        }

        gv.DataSource = dt;
        gv.DataBind();
    }

    protected void ddlProjectName_Changed(object sender, EventArgs e)
    {
        string sql = string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue);
        bindData(sql, gvListProject);
        clear();
    }
    protected void btnSave_click(object sender, EventArgs e)
    {
        if (chkTB(txtCustName) && chkTB(txtMeterNo))
        {
            try
            {
                string sCID = btnSave.CommandArgument;
                string sCol, sVal;
                if (string.IsNullOrEmpty(sCID))
                {
                    // Insert
                    sCol = "CID,PID,CName,[No],MeterNo,MeterDate,CreateDate";
                    sVal = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}"
                                    , conn.autoIdentity("tbCustomers", "CID")
                                    , ddlProjectName.SelectedValue
                                    , txtCustName.Text.Trim()
                                    , txtNo.Text.Trim()
                                    , string.IsNullOrEmpty(txtMeterNo.Text) ? "0" : txtMeterNo.Text.Trim()
                                    , "^NOW()"
                                    , "^NOW()");
                    conn.getInsertToDB("tbCustomers", sCol, sVal);
                    ltAlert.Text = alert.Infor("บันทึก เรียบร้อย");
                }
                else
                {
                    // Update
                    sCol = "CID,PID,CName,[No],MeterNo,MeterDate";
                    sVal = string.Format("{0}|{1}|{2}|{3}|{4}|{5}"
                                    , sCID
                                    , ddlProjectName.SelectedValue
                                    , txtCustName.Text.Trim()
                                    , txtNo.Text.Trim()
                                    , string.IsNullOrEmpty(txtMeterNo.Text) ? "0" : txtMeterNo.Text.Trim()
                                    , "^NOW()");
                    conn.getUpdateToDB("tbCustomers", sCol, sVal);
                    ltAlert.Text = alert.Infor("แก้ไข เรียบร้อย");
                }
                clear();
                txtCustName.Focus();
                bindData(string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue), gvListProject);
            }
            catch (Exception ex)
            {
                ltAlert.Text = alert.Error(ex.ToString());
            }
        }
        else
        {
            ltAlert.Text = alert.Warning("กรุณากรอกข้อมูลให้ครบ");
        }
    }
    protected void btnCancel_click(object sender, EventArgs e)
    {
        clear();
    }

    protected void chkAll_Changed(object sender, EventArgs e)
    {
        bool b = ((CheckBox)sender).Checked;
        foreach (GridViewRow row in gvListProject.Rows)
        {
            CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
            chkPrint.Checked = b;
        }
    }

    protected void btnPrint_click(object sender, EventArgs e)
    {
        try
        {
            ltAlert.Text = string.Empty;
            savePrint();
        }
        catch (Exception ex)
        {
            ltAlert.Text = alert.Error(ex.Message + "<br/>" + ex.InnerException);
        }
    }
    private void savePrint()
    {
        string sCID;
        string sMeterRate, sServiceRate, sMeterDate, sPrice;
        string sCol, sVal;
        List<string> liCID = new List<string>();
        foreach (GridViewRow row in gvListProject.Rows)
        {
            TextBox txtMeterNow = (TextBox)row.FindControl("txtMeterNow");
            LinkButton btnCName = (LinkButton)row.FindControl("btnCName");
            sCID = btnCName.CommandArgument;

            if (!string.IsNullOrEmpty(txtMeterNow.Text))
            {                
                TextBox txtMeterCurrent = (TextBox)row.FindControl("txtMeterCurrent");
                LinkButton MRate = (LinkButton)row.FindControl("MRate");
                LinkButton SRate = (LinkButton)row.FindControl("SRate");
                LinkButton MDate = (LinkButton)row.FindControl("MDate");

                sMeterRate = MRate.CommandArgument;
                sServiceRate = SRate.CommandArgument;
                sMeterDate = MDate.CommandArgument;
                sPrice = ((int.Parse(txtMeterNow.Text.Trim()) - int.Parse(txtMeterCurrent.Text.Trim())) * int.Parse(sMeterRate)).ToString();
                
                // Insert
                sCol = "CID,MeterNoBefore,MeterNoBeforeDate,MeterNo,MeterNoDate,MeterRate,Prices,ServiceRate";
                sVal = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}"
                                , sCID
                                , txtMeterCurrent.Text.Trim()
                                , sMeterDate
                                , txtMeterNow.Text.Trim()
                                , "^NOW()"
                                , sMeterRate
                                , sPrice
                                , sServiceRate);
                conn.getInsertToDB("tbTransactions", sCol, sVal);

                // Update
                sCol = "CID,PID,MeterNo,MeterDate";
                sVal = string.Format("{0}|{1}|{2}|{3}"
                                , sCID
                                , ddlProjectName.SelectedValue
                                , txtMeterNow.Text.Trim()
                                , "^NOW()");
                conn.getUpdateToDB("tbCustomers", sCol, sVal);
            }

            CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
            if (chkPrint.Checked)
            {
                liCID.Add(sCID);
            }
        }

        if (liCID.Count > 0)
        {
            // gen bills
            sCID = string.Empty;
            foreach (string s in liCID)
                sCID += string.Format(",{0}", s);

            sCID = sCID.Substring(1);

            string sql = string.Format(
            @"  select distinct c.CID, p.ProjectName, c.CName, c.No, 
                    t.MeterNoBefore, Format(t.MeterNoBeforeDate, 'dd/mm/yyyy') as MeterNoBeforeDate,
                    t.MeterNo, Format(t.MeterNoDate, 'dd/mm/yyyy') as MeterNoDate, 
                    t.Prices, t.MeterRate, t.ServiceRate
                from (tbCustomers c 
                    inner join tbProjects p on c.PID = p.PID)
                    inner join tbTransactions t on c.CID = t.CID and c.MeterNo = t.MeterNo and  Format(c.MeterDate, 'dd/mm/yyyy')  =  Format(t.MeterNoDate, 'dd/mm/yyyy') 
                where c.CID in ({0})"
            , sCID);

            Access_Conn conn = new Access_Conn();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = conn.getDataTable(sql);

            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("CrystalReport1.rpt"));
            rpt.SetDataSource(dt);

            string filePDF = string.Format("bill_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string pathPDF = Server.MapPath("bills/") + filePDF;
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pathPDF);

            string ss = string.Format("document.getElementById('pnlLoading').style.display = 'none'; window.open('{0}','_blank');"
                            , string.Format("/billservices/bills/{0}", filePDF));
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Open Bills", ss, true);
        }

        bindData(string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue), gvListProject);
    }
    protected void gvListProject_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName.ToString())
        {
            case "bEdit":
                showEdit(e.CommandArgument.ToString().Trim());
                break;

            default: break;
        }
    }
    protected void gvListProject_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /*
            LinkButton btnNo = (LinkButton)e.Row.FindControl("btnNo");
            if (btnNo != null)
            {
                btnNo.Text = DataBinder.Eval(e.Row.DataItem, "No").ToString();
                btnNo.CommandArgument = DataBinder.Eval(e.Row.DataItem, "CID").ToString().Trim();
            }
            */

            LinkButton btnCName = (LinkButton)e.Row.FindControl("btnCName");
            if (btnCName != null)
            {
                btnCName.Text = DataBinder.Eval(e.Row.DataItem, "CName").ToString();
                btnCName.CommandArgument = DataBinder.Eval(e.Row.DataItem, "CID").ToString().Trim();
            }

            LinkButton MRate = (LinkButton)e.Row.FindControl("MRate");
            if (MRate != null)
            {
                MRate.CommandArgument = DataBinder.Eval(e.Row.DataItem, "MeterRate").ToString().Trim();
            }

            LinkButton SRate = (LinkButton)e.Row.FindControl("SRate");
            if (SRate != null)
            {
                SRate.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ServiceRate").ToString().Trim();
            }

            LinkButton MDate = (LinkButton)e.Row.FindControl("MDate");
            if (MDate != null)
            {
                MDate.CommandArgument = ((DateTime)DataBinder.Eval(e.Row.DataItem, "MeterDate")).ToString("yyyy-MM-dd HH:mm:ss");
            }

            TextBox txtMeterCurrent = (TextBox)e.Row.FindControl("txtMeterCurrent");
            if (txtMeterCurrent != null)
            {
                txtMeterCurrent.Text = DataBinder.Eval(e.Row.DataItem, "MeterNo").ToString();
            }
        }
    }
    protected void gvListProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvListProject.PageIndex = e.NewPageIndex;
        bindData(string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue), gvListProject);
    }

    private void showEdit(string cID)
    {
        string sql = string.Format("{0} and c.CID = {1}", string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue), cID);
        System.Data.DataTable dt = new System.Data.DataTable();
        dt = conn.getDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            txtNo.Text = dt.Rows[0]["No"].ToString();
            txtCustName.Text = dt.Rows[0]["CName"].ToString();
            txtMeterNo.Text = dt.Rows[0]["MeterNo"].ToString();
            btnSave.CommandArgument = cID;
        }
        ltAlert.Text = string.Empty;
        dt.Dispose();
        conn.DisConnect();
    }

    private void clear()
    {
        conn.clearValue(pnlMasterForm, "ddlProjectName");
        ltAlert.Text = string.Empty;
        btnSave.CommandArgument = string.Empty;
    }
    private bool chkTB(TextBox txt)
    {
        return (txt.Text.Trim() != string.Empty);
    }
    private bool isNum(string inputTxt)
    {
        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("^([1-9]|[1-9][0-9]+)$");
        return re.IsMatch(inputTxt);
    }
    
}
