using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HW1
{
    public partial class Form1 : Form
    {
        DataSet ds;
        DataSet ds1;
        SqlDataAdapter adapter;
        SqlDataAdapter adapter1;
        SqlCommandBuilder commandBuilder;
        string connectionString = @"Data Source=DESKTOP-Q86IDH8;Initial Catalog=ProductCategory;Integrated Security=True";
        string sql = "select * from Product";
        string sql1 = "select * from Category";

        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AllowUserToAddRows = false;
            InitializeDataSet();
        }
        private void InitializeDataSet()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                adapter1 = new SqlDataAdapter(sql1, connection);
                ds = new DataSet();
                ds1 = new DataSet();
                adapter.Fill(ds);
                adapter1.Fill(ds1);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView2.DataSource = ds1.Tables[0];
                dataGridView2.Columns["id"].ReadOnly = true;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(row);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataRow row = ds1.Tables[0].NewRow();
            ds1.Tables[0].Rows.Add(row);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(row);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                commandBuilder = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = new SqlCommand("sp_CreateProduct", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@idCategory", SqlDbType.Int, 0, "idCategory"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 10, "name"));
                adapter.InsertCommand.Parameters.Add("@price", SqlDbType.Money, 0, "price");
                adapter.Update(ds);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter1 = new SqlDataAdapter(sql1, connection);
                commandBuilder = new SqlCommandBuilder(adapter1);
                adapter1.InsertCommand = new SqlCommand("sp_CreateCategory", connection);
                adapter1.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter1.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 10, "name"));
                SqlParameter parameter = adapter1.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 0, "id");
                parameter.Direction = ParameterDirection.Output;
                adapter1.Update(ds1);
            }
        }
    }
}
