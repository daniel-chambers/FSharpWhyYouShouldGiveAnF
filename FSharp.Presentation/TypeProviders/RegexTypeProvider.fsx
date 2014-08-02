#r @"..\packages\FSharpx.TypeProviders.Regex.1.8.41\lib\40\FSharpx.TypeProviders.Regex.dll"

type PhoneRegex = FSharpx.Regex< @"^\((?<Area>\d{2})\) (?<Number>\d{4} ?\d{4})$" >

let parseAndPrintNumber number =
    let result = number |> PhoneRegex().Match
    match result.Success with
    | true -> 
        printfn "Area: %s" result.Area.Value
        printfn "Number: %s" result.Number.Value
    | false -> printfn "Not a phone number"

"(03) 9373 3242" |> parseAndPrintNumber