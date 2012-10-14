type Tree<'a> = Empty | Node of 'a * Tree<'a> * Tree<'a>

let rec iterTree f = function
    | Empty -> ()
    | Node (key, left, right) ->
        iterTree f left
        f key
        iterTree f right

let rec treeAdd key = function
    | Empty -> Node (key, Empty, Empty)
    | Node (key', left, right) as cur ->
        if key' = key then cur
        elif key < key' then
            Node (key', treeAdd key left, right)
        else Node (key', left, treeAdd key right)

let rec treeFind key = function
    | Empty -> false
    | Node (key', left, right) as cur ->
        if key' = key then true
        elif key < key' then
            treeFind key left
        else treeFind key right

let deleteNode =
    let rec findSmallestRight = function
        | Empty -> failwith "findSmallestRight can't be applied to empty tree"
        | Node (key, Empty, right) -> key, right
        | Node (key, left, right) ->
            let smallest, leftTree = findSmallestRight left
            smallest, Node (key, leftTree, right)
    function 
    | Empty -> failwith "deleteNode can't be applied to empty tree"
    | Node (_, left, right) ->
        match left with
        | Empty -> right
        | _ ->
            match right with
            | Empty -> left
            | _ -> 
               let smallest, rightTree = findSmallestRight right
               Node (smallest, left, rightTree)

let rec treeDelete key = function
    | Empty -> Empty
    | Node (key', left, right) as cur ->
        if key' = key then
            deleteNode cur
        elif key < key' then
            treeDelete key left
        else treeDelete key right

let swap f a b = f b a

let tree = 
    [1; 100; 23; 24; 16; -4; 40; 20; 50; 70]
    |> List.fold (swap treeAdd) Empty

tree |> iterTree (printf "%d ")
printfn ""

[1; 100; 23; 24; 16; -4; 40; 20; 50; 70]
|> List.map (swap treeFind tree)
|> List.iter (printf "%A ")
printfn ""

[5; 99; 21; 25; 33; 43; 11; 1000; -50; 72]
|> List.map (swap treeFind tree)
|> List.iter (printf "%A ")
printfn ""

let treeCleared = 
    [1; 100; 23; 24; 16; -4; 40; 20; 50; 70]
    |> List.fold (swap treeDelete) tree

treeCleared |> iterTree (printf "%d ")
printfn ""
