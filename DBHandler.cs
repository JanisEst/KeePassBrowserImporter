using System;
using System.Data;
using System.Data.SQLite;

namespace KeePassBrowserImporter
{
	/// <summary>A helper class for SQLite databases.</summary>
	internal class DBHandler : IDisposable
	{
		private SQLiteConnection connection;

		public DBHandler(string database)
		{
			connection = new SQLiteConnection("FailIfMissing=True; Data Source=" + database);
			connection.Open();
		}

		~DBHandler()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		public void Dispose(bool free)
		{
			if (connection != null)
			{
				connection.Close();
				connection = null;
			}
		}

		/// <summary>
		/// Execute the query and store the result in a DataTable
		/// </summary>
		/// <param name="result">[out] The datatable for the result</param>
		/// <param name="query">The query to execute</param>
		public void Query(out DataTable result, string query)
		{
			using (var adapter = new SQLiteDataAdapter(query, connection))
			{
				var ds = new DataSet();

				adapter.Fill(ds);

				result = ds.Tables[0];
			}
		}
	}
}
