using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    Access_Conn conn = new Access_Conn();
    private const string _ssid = "QryGVListProject";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Remove(_ssid);
            Session[_ssid] = "select PID,ProjectName,MeterRate,ServiceRate from tbProjects";

            clear();
        }
        try { bindData(Session[_ssid].ToString(), gvListProject); }
        catch (Exception ex) { string sssss = ex.ToString(); Response.Write(sssss); }
    }

    private void bindData(string sql, GridView gv)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        dt = conn.getDataTable(sql);

        gv.DataSource = dt;
        gv.DataBind();
    }
    protected void btnSave_click(object sender, EventArgs e)
    {
        if (chkTB(txtProjectName) && chkTB(txtMeterUnit) && chkTB(txtService))
        {
            try
            {
                string sPID = btnSave.CommandArgument;
                string sCol, sVal;
                if (string.IsNullOrEmpty(sPID))
                {
                    // Insert
                    sCol = "PID,ProjectName,MeterRate,ServiceRate,CreateDate";
                    sVal = string.Format("{0}|{1}|{2}|{3}|{4}"
                                    , conn.autoIdentity("tbProjects", "PID")
                                    , txtProjectName.Text.Trim()
                                    , txtMeterUnit.Text.Trim()
                                    , txtService.Text.Trim()
                                    , "^NOW()");
                    conn.getInsertToDB("tbProjects", sCol, sVal);
                    ltAlert.Text = alert.Infor("บันทึก เรียบร้อย");
                }
                else
                {
                    // Update
                    sCol = "PID,ProjectName,MeterRate,ServiceRate";
                    sVal = string.Format("{0}|{1}|{2}|{3}"
                                    , sPID
                                    , txtProjectName.Text.Trim()
                                    , txtMeterUnit.Text.Trim()
                                    , txtService.Text.Trim());
                    conn.getUpdateToDB("tbProjects", sCol, sVal);
                    ltAlert.Text = alert.Infor("แก้ไข เรียบร้อย");
                }
                clear();
                bindData(Session[_ssid].ToString(), gvListProject);
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
            LinkButton btnProjectName = (LinkButton)e.Row.FindControl("btnProjectName");
            if (btnProjectName != null)
            {
                btnProjectName.Text = DataBinder.Eval(e.Row.DataItem, "ProjectName").ToString();
                btnProjectName.CommandArgument = DataBinder.Eval(e.Row.DataItem, "PID").ToString().Trim();
            }
        }
    }
    protected void gvListProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvListProject.PageIndex = e.NewPageIndex;
        bindData(Session[_ssid].ToString(), gvListProject);
    }    

    private void showEdit(string pID)
    {
        string sql = string.Format(@"{0} where PID={1}"
                                    , Session[_ssid].ToString()
                                    , pID);
        System.Data.DataTable dt = new System.Data.DataTable();
        dt = conn.getDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            txtProjectName.Text = dt.Rows[0]["ProjectName"].ToString();
            txtMeterUnit.Text = dt.Rows[0]["MeterRate"].ToString();
            txtService.Text = dt.Rows[0]["ServiceRate"].ToString();

            btnSave.CommandArgument = pID;
        }
        ltAlert.Text = string.Empty;
        dt.Dispose();
        conn.DisConnect();
    }

    private void clear()
    {
        conn.clearValue(pnlMasterForm);
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
