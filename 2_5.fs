let isPalindrom (s : string) =
    let rec inner i j =
        if i >= j then true
        elif s.[i] <> s.[j] then false
        else inner (i+1) (j-1)
    inner 0 (s.Length - 1)

printfn "%A" <| List.map isPalindrom ["abcba"; "abba"; "abcda"; "abcbd"]