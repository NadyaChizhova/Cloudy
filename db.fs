open System.Data.SqlClient

let sqlConn = "server=nadusha_pc\\sqlexpress; Integrated Security=True; Database=Movies"

let connDB = new SqlConnection(sqlConn)
connDB.Open()
let sql = "SELECT * FROM Movies"
let cmd = new SqlCommand(sql, connDB)

let reader = cmd.ExecuteReader()
while reader.Read() do
    let values = Array.init (reader.FieldCount) (fun i -> reader.[i])
    values |> printfn "%A"
reader.Close()

let _ =
        let sql = "Insert into Movies values ('aaaaaa')"
        let cmd = new SqlCommand(sql, connDB)
        cmd.ExecuteNonQuery()
