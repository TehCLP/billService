using System;
using System.Collections.Generic;
using System.Web;

using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Access_Conn
/// </summary>
public class Access_Conn
{
    public OleDbConnection conn = new OleDbConnection();
    public OleDbCommand cmd;
    public string strConnectionString;
	public Access_Conn()
	{
        strConnectionString = ConfigurationManager.ConnectionStrings["accessDB"].ConnectionString.ToString();
	}
    public void Connect()
    {
        conn = new OleDbConnection(strConnectionString);
        conn.Open();
    }
    public void DisConnect()
    {
        conn.Close();
    }

    public void ExeTransDB(string sql)
    {
        if (conn.State == ConnectionState.Closed) Connect();
        cmd = new OleDbCommand(sql, conn);
        cmd.ExecuteNonQuery();
        DisConnect();
    }

    public DataTable getDataTable(string sql)
    {
        try
        {
            if (conn.State == ConnectionState.Closed) Connect();
            OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
            //da.SelectCommand.Parameters[0].Value = "";
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }
        finally
        {
            DisConnect();
        }
    }
    public void getDataToDDL(ref DropDownList ddl, string sql, string textField, string valueField)
    {
        try
        {
            if (conn.State == ConnectionState.Closed) Connect();

            OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Table");

            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = ds.Tables[0].Columns[textField].ColumnName.ToString();
            ddl.DataValueField = ds.Tables[0].Columns[valueField].ColumnName.ToString();
            ddl.DataBind();

            ds.Dispose();
        }
        finally
        {
            DisConnect();
        }
    }

    public void getInsertToDB(string tblname, string strCol, string strVal)
    {
        string strTemp;
        string sCheckInsertSingleCote = "^"; // insert ^ before value mean not insert ' cover value
        string[] arrValTemp = strVal.Split('|');

        for (int j = 0; j < arrValTemp.Length; j++)
        {
            arrValTemp[j] = arrValTemp[j].Replace("'", "''");
        }

        strTemp = "Insert into " + tblname + " (" + strCol + ") values(";

        if (arrValTemp.Length == 1)
        {
            strTemp = strTemp + (arrValTemp[0].StartsWith(sCheckInsertSingleCote) ? arrValTemp[0].Substring(1) : string.Format("'{0}'", arrValTemp[0]));
        }
        else
        {
            for (int i = 0; i < arrValTemp.Length; i++)
            {
                if (i == 0) { strTemp = strTemp + (arrValTemp[i].StartsWith(sCheckInsertSingleCote) ? arrValTemp[i].Substring(1) : string.Format("'{0}'", arrValTemp[i])); }
                else if (i == arrValTemp.Length - 1)
                {
                    strTemp = strTemp + "," + (arrValTemp[i].StartsWith(sCheckInsertSingleCote) ? arrValTemp[i].Substring(1) : string.Format("'{0}'", arrValTemp[i])) + ")";
                }
                else if (i > 0)
                {
                    strTemp = strTemp + "," + (arrValTemp[i].StartsWith(sCheckInsertSingleCote) ? arrValTemp[i].Substring(1) : string.Format("'{0}'", arrValTemp[i]));
                }
            }
        }
        ExeTransDB(strTemp);
    }
    public void getUpdateToDB(string tblname, string strCol, string strVal)
    {
        // first strCol and strVal get to add condition where
        // strCol และ strVal ค่าแรก เอาไว้ใส่ เงื่อนไข where
        string sCheckInsertSingleCote = "^"; // insert ^ before value mean not insert ' cover value
        string strSql = "Update " + tblname + " set ";
        string[] arrColTemp = strCol.Split(',');
        string[] arrValTemp = strVal.Split('|');

        for (int j = 0; j < arrValTemp.Length; j++)
        {
            arrValTemp[j] = arrValTemp[j].Replace("'", "''");
        }

        for (int i = 0; i < arrColTemp.Length; i++)
        {
            if (arrColTemp[i] != "" && arrValTemp[i] != "")
            {
                if (i == 1)
                    strSql = strSql + arrColTemp[i] + "=" + (arrValTemp[i].StartsWith(sCheckInsertSingleCote) ? arrValTemp[i].Substring(1) : string.Format("'{0}'", arrValTemp[i]));

                else if (i > 1)
                    strSql = strSql + "," + arrColTemp[i] + "=" + (arrValTemp[i].StartsWith(sCheckInsertSingleCote) ? arrValTemp[i].Substring(1) : string.Format("'{0}'", arrValTemp[i]));
            }
        }
        strSql = strSql + " where " + arrColTemp[0] + " =" + arrValTemp[0] + "";

        ExeTransDB(strSql);
    }

    public string getSelectOne(string sql, string colname)
    {
        DataTable dt;
        OleDbDataAdapter da;

        try
        {
            if (conn.State == ConnectionState.Closed) Connect();

            da = new OleDbDataAdapter(sql, conn);
            dt = new DataTable();
            da.Fill(dt);
        }
        finally
        {
            DisConnect();
        }

        if (dt.Rows.Count == 0) return "";

        return dt.Rows[0][colname].ToString();
    }
    public string getSelectOne(string tbl, string col, string whr)
    {
        DataTable dt;
        OleDbDataAdapter da;

        try
        {
            string sql = string.Format("select {0} from {1}{2}", col, tbl, ((whr.Trim() == "") ? "" : " where " + whr));

            if (conn.State == ConnectionState.Closed) Connect();

            da = new OleDbDataAdapter(sql, conn);
            dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0) return "";

            return dt.Rows[0][col].ToString();
        }
        finally
        {
            dt = null;
            DisConnect();
        }
    }

    public string autoIdentity(string tblName, string colName)
    {
        string strSql = string.Format("select max({0}) as {0} from {1}", colName, tblName);
        string tmp = getSelectOne(strSql, colName);
        try { return tmp = (tmp == string.Empty) ? "1" : ((int.Parse(tmp) + 1).ToString()); }
        catch { return string.Format("Error Column {0} : Isn't Type Int", colName); }
    }
    public void clearValue(Panel pnl)
    {
        foreach (System.Web.UI.Control ctr in pnl.Controls)
        {
            if (ctr is TextBox)
            {
                (ctr as TextBox).Text = string.Empty;
                //(ctr as TextBox).Attributes.Clear();
            }
            else if (ctr is DropDownList)
                (ctr as DropDownList).SelectedIndex = 0;
            else if (ctr is CheckBox)
                (ctr as CheckBox).Checked = false;
        }
    }
    public void clearValue(Panel pnl, params string[] idIgnoreClear)
    {
        foreach (System.Web.UI.Control ctr in pnl.Controls)
        {
            if (ctr is TextBox)
            {

                if (Array.IndexOf(idIgnoreClear, (ctr as TextBox).ID) < 0)
                {
                    (ctr as TextBox).Text = string.Empty;
                    //(ctr as TextBox).Attributes.Clear();
                }
            }
            else if (ctr is DropDownList)
            {
                if (Array.IndexOf(idIgnoreClear, (ctr as DropDownList).ID) < 0)
                {
                    (ctr as DropDownList).SelectedIndex = 0;
                }
            }
            else if (ctr is CheckBox)
            {
                if (Array.IndexOf(idIgnoreClear, (ctr as CheckBox).ID) < 0)
                {
                    (ctr as CheckBox).Checked = false;
                }
            }
        }
    }
    public bool isDate(string sDate)
    {
        bool b = false;
        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
        if (re.IsMatch(sDate))
        {
            try
            {
                DateTime dt = DateTime.Parse(sDate);
                b = true;
            }
            catch { }
        }
        return b;
    }
}
