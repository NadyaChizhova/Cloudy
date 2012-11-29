﻿namespace Website

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Sitelets
open IntelliFactory.WebSharper.Web

module DB = 
    open System.Data.SqlClient

    let sqlConn = "Data Source=.\SQLEXPRESS;Initial Catalog=Movies;Integrated Security=True"

    let moviesDB = new SqlConnection(sqlConn)
    moviesDB.Open()
    [<Rpc>]
    let selectAll =
        let sql = "SELECT * FROM Movie"
        let cmd = new SqlCommand(sql, moviesDB)
        fun () ->
            let res = new ResizeArray<_>()
            let reader = cmd.ExecuteReader()
            while reader.Read() do
                res.Add <| Array.init (reader.FieldCount) (fun i -> reader.[i])
            reader.Close()
            res.ToArray()

    [<Rpc>]
    let addValue value =
        let sql = "Insert into Movie values ('" + value + "')"
        let cmd = new SqlCommand(sql, moviesDB)
        cmd.ExecuteNonQuery()

    [<Rpc>]
    let checkLogin (login : string) (password : string) =
        let sql = "Select CASE WHEN EXISTS (SELECT * FROM [User] us WHERE us.Login = @login and us.Password = @password) THEN 1 ELSE 0 END"
        let cmd = new SqlCommand(sql, moviesDB)
        cmd.Parameters.AddWithValue("@login", login) |> ignore
        cmd.Parameters.AddWithValue("@password", password) |> ignore
        unbox<int> (cmd.ExecuteScalar()) <> 0
        

/// This module defines the client-side functionality used by the website.
module Forms =

    type LoginInfo =
        {
            Name : string
            Password : string
        }

    [<JavaScript>]
    let Input (label: string) (err: string) =
        Controls.Input ""
        |> Validator.IsNotEmpty err
        |> Enhance.WithValidationIcon
        |> Enhance.WithTextLabel label

    [<JavaScript>]
    let InputInt (label: string) (err: string) =
        Controls.Input ""
        |> Validator.IsInt err
        |> Enhance.WithValidationIcon
        |> Enhance.WithTextLabel label
        |> Formlet.Map int

    [<JavaScript>]
    let MovieTitleForm () : Formlet<string> =
        Formlet.Yield (fun name-> name)
        <*> Input "Title" "Please enter a title"

    [<JavaScript>]
    let AddMovie () =
        let textForm =
            MovieTitleForm ()
            |> Enhance.WithSubmitButton
            |> Enhance.WithCustomFormContainer {
                 Enhance.FormContainerConfiguration.Default with
                    Header =
                        "Type Movie title to add"
                        |> Enhance.FormPart.Text
                        |> Some
                    Description =
                        "Please enter movie title."
                        |> Enhance.FormPart.Text
                        |> Some
               }
        let proc (title : string) () =
            ignore <| DB.addValue title
            FieldSet [
                P ["Title is succesfully added" |> Text]
            ]

        Formlet.Do {
            let! t = textForm
            return! Formlet.OfElement (proc t)
        }
        |> Formlet.Flowlet
        

    [<JavaScript>]
    let WarningPanel label =
        Formlet.Do {
            let! _ =
                Formlet.OfElement <| fun _ ->
                    Div [Attr.Class "warningPanel"] -< [Text label]
            return! Formlet.Never ()
        }

    [<JavaScript>]
    let WithLoadingPane (a: Async<'T>) (f: 'T -> Formlet<'U>) : Formlet<'U> =
        let loadingPane =
            Formlet.BuildFormlet <| fun _ ->
                let elem = 
                    Div [Attr.Class "loadingPane"]
                let state = new Event<Result<'T>>()
                async {
                    let! x = a
                    do state.Trigger (Result.Success x)
                    return ()
                }
                |> Async.Start
                elem, ignore, state.Publish
        Formlet.Replace loadingPane f
    
    [<Inline "window.location = $url">]
    let Redirect (url: string) = ()

    [<Rpc>]
    let Login (loginInfo: LoginInfo) =
        System.Threading.Thread.Sleep 1000
        if DB.checkLogin loginInfo.Name loginInfo.Password then
            UserSession.LoginUser loginInfo.Name
            true
        else
            false
        |> async.Return

    [<JavaScript>]
    let LoginForm (redirectUrl: string) : Formlet<unit> =
        let uName =
            Controls.Input ""
            |> Validator.IsNotEmpty "Enter Username"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Username"
        let pw =
            Controls.Password ""
            |> Validator.IsNotEmpty "Enter Password"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Password"
        let loginF =
            Formlet.Yield (fun n pw -> {Name=n; Password=pw})
            <*> uName <*> pw

        Formlet.Do {
            let! uInfo = 
                loginF
                |> Enhance.WithCustomSubmitAndResetButtons
                    {Enhance.FormButtonConfiguration.Default with Label = Some "Login"}
                    {Enhance.FormButtonConfiguration.Default with Label = Some "Reset"}
            return!
                WithLoadingPane (Login uInfo) <| fun loggedIn ->
                    if loggedIn then
                        Redirect redirectUrl
                        Formlet.Return ()
                    else
                        WarningPanel "Login failed"
        }
        |> Enhance.WithFormContainer

/// Add new movie.
type AddMovieControl() =
    inherit IntelliFactory.WebSharper.Web.Control ()

    [<JavaScript>]
    override this.Body = Forms.AddMovie () :> _

/// Exposes the signup form so that it can be used in sitelets.
type LoginControl(redirectUrl: string) =
    inherit IntelliFactory.WebSharper.Web.Control ()

    new () = new LoginControl("?")
    [<JavaScript>]
    override this.Body = Forms.LoginForm redirectUrl :> _
