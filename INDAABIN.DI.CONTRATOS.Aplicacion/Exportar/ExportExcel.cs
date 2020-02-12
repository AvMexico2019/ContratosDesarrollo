using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Exportar
{
    public class ExportExcel
    {
        Table table;
        TableRow row;
        TableCell cell;

        public ExportExcel()
        {
            table = new Table();

            row = new TableRow();
            cell = new TableCell();
        }

        public ExportExcel(string title)
        {            
            table = new Table();

            row = new TableRow();
            cell = new TableCell();

            cell.Text = title;

            row.Cells.Add(cell);
            table.Rows.Add(row);
        }

        public void AddSection(Control control)
        {
            row = new TableRow();
            cell = new TableCell();

            cell.Controls.Add(control);

            row.Cells.Add(cell);
            table.Rows.Add(row);
        }
                
        public StringWriter GetReport()
        {
            StringWriter tw = null;
            HtmlTextWriter hw = null;

            //Crea el HTML usando como base la tabla
            try
            {
                tw = new StringWriter();
                hw = new HtmlTextWriter(tw);

                PrepareControlForExport(table);
                
                table.RenderControl(hw);

                return tw;
            }
            catch (Exception ex)
            {
                if (hw != null)
                {
                    return tw;
                }

                return new StringWriter();
            }
        }
        public void SendToBrowser(HttpContext context)
        {
            string html = "";

            try
            {
                context.Response.Clear();

                context.Response.Buffer = true;

                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.ContentEncoding = System.Text.Encoding.Default;
                context.Response.Charset = "";

               

                html =  GetReport().ToString();

                context.Response.Write(html);
                context.Response.End();
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
            catch (Exception generalEx)
            {

            }
        }
        
        public void SendToBrowser(HttpContext context, string fileName)
        {
            try            
            {
                //html = @"<table border=""0"">Prueba<table/>";
                context.Response.Clear();
                context.Response.ClearContent();
                context.Response.ClearHeaders();
                context.Response.Buffer = false;
                context.Response.BufferOutput = false;

                context.Response.ContentType = "application/vnd.ms-excel"; //"application/vnd.ms-excel";
                context.Response.AddHeader("content-disposition", "inline; filename=" + fileName + ".xls");
                context.Response.ContentEncoding = System.Text.Encoding.Default;
                context.Response.Charset = "UTF-8";

                //html += GetReport().ToString();

                context.Response.Write(GetReport().ToString());
                context.Response.Flush();
                context.Response.End();
            }
            catch (Exception generalEx)
            {
                string mensaje = generalEx.Message;
            }
        }
        
        public void SendToBrowser(HttpContext context, string fileName, string sCadenaFiltros)
        {
            string html = "";

            try
            {
                html = @"<table border=""0"">" + sCadenaFiltros + "<table/>";
                context.Response.Clear();
                context.Response.ClearContent();
                context.Response.ClearHeaders();
                context.Response.Buffer = false;
                context.Response.BufferOutput = false;

                context.Response.ContentType = "application/vnd.ms-excel"; //"application/vnd.ms-excel";
                context.Response.AddHeader("content-disposition", "inline; filename=" + fileName + ".xls");
                context.Response.ContentEncoding = System.Text.Encoding.Default;
                context.Response.Charset = "UTF-8";

                html += GetReport().ToString();

                context.Response.Write(html);
                context.Response.Flush();
                context.Response.End();
            }
            //catch (System.Threading.ThreadAbortException ex)
            //{
            //    string mensaje = ex.Message;
            //}
            catch (Exception generalEx)
            {
                string mensaje = generalEx.Message;
            }
        }

        public void SendToBrowser(string pHTML, string fileName)
        {
            string html = "";

            try
            {

                HttpContext.Current.Response.Clear();

                HttpContext.Current.Response.Buffer = false;

                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("content-disposition", "inline; filename=" + fileName + ".xls");
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
                HttpContext.Current.Response.Charset = "utf-8";

                //html = GetReport().ToString();

                HttpContext.Current.Response.Write(pHTML);
                HttpContext.Current.Response.End();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                string mensaje = ex.Message;
            }
            catch (Exception generalEx)
            {

            }
        }

        private void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];

                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is Button)
                {
                    control.Controls.Remove(current);
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }
                //else if (current is System.Web.UI.ExtenderControl)
                //{
                //    control.Controls.Remove(current);
                //}

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }
    }
}