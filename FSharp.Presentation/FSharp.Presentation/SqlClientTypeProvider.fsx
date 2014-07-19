#r @"..\packages\FSharp.Data.SqlClient.1.2.27\lib\net40\FSharp.Data.SqlClient.dll"

open FSharp.Data

[<Literal>]
let connectionString = @"Data Source=localhost;Initial Catalog=AdventureWorksLT2012;Integrated Security=True"

[<Literal>]
let sql = "
    SELECT TOP (@TopN) c.CustomerID, c.FirstName, c.LastName, SUM(soh.TotalDue) AS TotalDue
    FROM SalesLT.Customer c
    INNER JOIN SalesLT.SalesOrderHeader soh ON c.CustomerID = soh.CustomerID
    GROUP BY c.CustomerID, c.FirstName, c.LastName
    HAVING SUM(soh.TotalDue) > @OrdersMoreThan
    ORDER BY SUM(soh.TotalDue) DESC
"

type TopCustomerQuery = SqlCommandProvider<sql, connectionString>
let query = new TopCustomerQuery()

async {
    let! results = query.AsyncExecute(TopN = 5L, OrdersMoreThan = 1000m)
    for result in results do
        printfn "%s %s: %.2f" result.FirstName result.LastName result.TotalDue.Value
}
|> Async.RunSynchronously