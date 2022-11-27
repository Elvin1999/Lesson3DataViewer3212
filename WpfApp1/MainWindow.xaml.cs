using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //using (var connection=new SqlConnection())
            //{
            //    connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            //    DataSet set = new DataSet();
            //    var da = new SqlDataAdapter("SELECT Id,Name,Pages,YearPress FROM Books", connection);

            //    da.Fill(set,"Books");
            //    DataViewManager dvm=new DataViewManager(set);
            //   // dvm.DataViewSettings["Books"].RowFilter = "Pages>500";
            //    dvm.DataViewSettings["Books"].RowFilter = "YearPress>2000 AND Pages>500";
            //    dvm.DataViewSettings["Books"].Sort = "YearPress DESC";

            //    DataView dv = dvm.CreateDataView(set.Tables["Books"]);

            //    mygrid.ItemsSource = dv;

            //    //mygrid.ItemsSource = set.Tables[0].DefaultView;
            //}




            #region Transaction

            SqlTransaction sqlTransaction = null;
            using(var connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
                connection.Open();

                sqlTransaction= connection.BeginTransaction();

                SqlCommand comm1 = new SqlCommand("INSERT INTO Press(Id,Name) VALUES(@Id,@Name)", connection);
                comm1.Transaction = sqlTransaction;

                SqlParameter param1 = new SqlParameter();
                param1.Value = 5566;
                param1.ParameterName = "@Id";
                param1.SqlDbType = SqlDbType.Int;

                SqlParameter param2 = new SqlParameter();
                param2.Value = "John";
                param2.ParameterName = "@Name";
                param2.SqlDbType = SqlDbType.NVarChar;

                comm1.Parameters.Add(param1);
                comm1.Parameters.Add(param2);



                SqlCommand comm2 = new SqlCommand("sp_UpdateBook",connection);
                comm2.Transaction = sqlTransaction;
                comm2.CommandType = CommandType.StoredProcedure;

                var p1 = new SqlParameter();
                p1.Value = 1;
                p1.ParameterName = "@MyId";
                p1.SqlDbType = SqlDbType.Int;


                var p2 = new SqlParameter();
                p2.Value = 55555;
                p2.ParameterName = "@Page";
                p2.SqlDbType = SqlDbType.Int;

                comm2.Parameters.Add(p1);
                comm2.Parameters.Add(p2);


                try
                {
                    comm1.ExecuteNonQuery();
                    comm2.ExecuteNonQuery();
                    sqlTransaction.Commit();



                    #region GetData
                        DataSet set = new DataSet();
                        var da = new SqlDataAdapter("SELECT Id,Name,Pages,YearPress FROM Books", connection);

                        da.Fill(set, "Books");
                        DataViewManager dvm = new DataViewManager(set);
                        dvm.DataViewSettings["Books"].Sort = "YearPress DESC";

                        DataView dv = dvm.CreateDataView(set.Tables["Books"]);

                        mygrid.ItemsSource = dv;
                    #endregion



                    //MessageBox.Show("Okay");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    sqlTransaction.Rollback();
                }


            }


            #endregion


        }
    }
}
