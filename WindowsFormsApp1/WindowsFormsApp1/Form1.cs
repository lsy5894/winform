using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataTable table = new DataTable();
        private void button1_Click(object sender, EventArgs e)
        {

           // DataTable table = new DataTable();

            // column을 추가합니다.
            table.Columns.Add("일시", typeof(string));
            table.Columns.Add("julainday", typeof(string));
            table.Columns.Add("조위", typeof(string));


       
            // 각각의 행에 내용을 입력합니다.
            table.Rows.Add("2023-01-01 00:00", "1", "0.20");
            table.Rows.Add("2023-01-01 00:10", "2", "0.17");
            table.Rows.Add("2023-02-01 00:20", "3", "-0.15");
            table.Rows.Add("2023-02-01 00:30", "5", "-0.12");
            table.Rows.Add("2023-03-01 00:40", "1", "0.12");
            table.Rows.Add("2023-04-01 00:50", "3", "-0.10");
            table.Rows.Add("2023-05-01 01:00", "4", "0.15");
            table.Rows.Add("2023-06-01 01:10", "2", "0.17");
            table.Rows.Add("2023-06-01 01:20", "3", "-0.19");
            table.Rows.Add("2023-07-01 03:20", "5", "-0.19");
            table.Rows.Add("2023-08-01 05:20", "2", "0.18");
            table.Rows.Add("2023-09-01 06:20", "1", "0.14");
            table.Rows.Add("2023-09-01 07:10", "2", "0.19");
            table.Rows.Add("2023-010-01 07:20", "3", "0.15");
            table.Rows.Add("2023-011-01 08:20", "4", "0.23");
            table.Rows.Add("2023-12-01 09:20", "2", "0.25");
            table.Rows.Add("2023-12-01 09:30", "1", "0.24");
            // 값들이 입력된 테이블을 DataGridView에 입력합니다.
            dataGridView1.DataSource = table;

            dataGridView1.Columns[0].Frozen = true;
            dataGridView1.Columns[0].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            //dataGridView1.Columns[0].DefaultCellStyle.Format = "yyyy-MM-dd HH:00";


            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true; // // X-축 zoom : 속성 설정. 마우스 클릭 & 드래그       
            chart1.Series[0] = new Series();
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[0].XValueMember = dataGridView1.Columns[0].DataPropertyName;
            chart1.Series[0].YValueMembers = dataGridView1.Columns[1].DataPropertyName;

            chart1.DataSource = dataGridView1.DataSource;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45;// x축 45도 회전
            chart1.Series[0].Name = "조위";


        }
        DateTimePicker dateTimePicker1;

        private void DateTimePickerChange(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell.Value = dateTimePicker1.Text.ToString();
            MessageBox.Show(string.Format("Date changed to {0}", dateTimePicker1.Text.ToString()));
        }

        private void DateTimePickerClose(object sender, EventArgs e)
        {
            dateTimePicker1.Visible = false;
        }

  

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Check the cell clicked is not the column header cell
            if (e.RowIndex != -1) //header 아닌지 check
            {
                // Apply on column index in which you want to display DatetimePicker.            
                if (e.ColumnIndex == 0)
                {
                    dateTimePicker1 = new DateTimePicker();  // dateTimePicker 초기화                                                      
                    dataGridView1.Controls.Add(dateTimePicker1); //dataGridView1에 dateTimePicker1 추가 

                    // Setting the format i.e. mm/dd/yyyy)
                    dateTimePicker1.Format = DateTimePickerFormat.Short;


                    // dateTimeOffsetEdit1.CustomDisplayText = "yyyy-MM-dd HH:mm:ss ";

                    dateTimePicker1.Format = DateTimePickerFormat.Custom;
                    dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss "; //2018-01-01 12:30
                                                                           // dateTimePicker1.ShowUpDown = true;

                    // Create retangular area that represents the display area for a cell.
                    Rectangle oRectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                    dateTimePicker1.Size = new Size(oRectangle.Width, oRectangle.Height);
                    // Setting location for dateTimePicker1.
                    dateTimePicker1.Location = new Point(oRectangle.X, oRectangle.Y);
                    // An event attached to dateTimePicker1 which is fired when any date is selected.
                    dateTimePicker1.TextChanged += new EventHandler(DateTimePickerChange);
                    // An event attached to dateTimePicker1 which is fired when DateTimeControl is closed.
                    dateTimePicker1.CloseUp += new EventHandler(DateTimePickerClose);

                }
            }

        }

        // Point? prevPosition = null;
        // ToolTip tooltip = new ToolTip();

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();

        
         private void chart1_MouseMove(object sender, MouseEventArgs e)
         {
             var pos = e.Location;
             if (prevPosition.HasValue && pos == prevPosition.Value)
                 return;
             tooltip.RemoveAll();
             prevPosition = pos;

             var results = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint); // set ChartElementType.PlottingArea for full area, not only DataPoints
             foreach (var result in results)
             {
                 if (result.ChartElementType == ChartElementType.DataPoint) // set ChartElementType.PlottingArea for full area, not only DataPoints
                 {
                    var prop = result.Object as DataPoint;

                    var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);
                    // var xVal = result.ChartArea.AxisY.PixelPositionToValue(pos.X);

                    tooltip.Show(((int)yVal).ToString(), chart1, pos.X, pos.Y - 15);
                    tooltip.Show("X=" + prop.XValue + ", Y=" + prop.YValues[0], this.chart1,
                                           pos.X, pos.Y - 15);
                }
             }
         }

        private void button2_Click(object sender, EventArgs e)
        {
            //DateTimePicker dateTimePicker2 = new DateTimePicker();
            //DateTimePicker dateTimePicker3 = new DateTimePicker();

            dateTimePicker2.Visible = true;
            dateTimePicker3.Visible = true;

          

            

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
                       
           
          
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            table.DefaultView.RowFilter = "일시 >= '" + dateTimePicker2.Value.Date + "' and  일시 <= '" +
            dateTimePicker3.Value.Date + "'";


            // dataGridView1.DataSource = table;
            //chart1.DataSource = table;


            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";

            //chart1.DataSource = dataGridView1.DataSource;
            //xyDiagram.AxisY.WholeRange.SetMinMaxValues(-1, 1); Y축범위 설정
            DateTime atime = dateTimePicker2.Value.Date;
            DateTime btime = dateTimePicker3.Value.Date;
             chart1.ChartAreas[0].AxisX.Minimum = atime.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = btime.ToOADate();

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
