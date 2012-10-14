let isPrime n =
    let rec inner i =
        if i * i > n then true
        elif n % i = 0 then false
        else inner (i+1)
    inner 2

let primes = seq {
    let i = ref 0
    while true do
        if isPrime !i then yield !i
        incr i}

primes |> Seq.take 20 |> Seq.iter (printf "%d ")