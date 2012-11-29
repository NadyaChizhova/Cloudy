﻿namespace Website

open System
open System.IO
open System.Web
open IntelliFactory.WebSharper.Sitelets

/// The website definition.
module SampleSite =
    open IntelliFactory.Html
    open IntelliFactory.WebSharper

    /// Actions that corresponds to the different pages in the site.
    type Action =
        | Home
        | AddMovie
        | Login of option<Action>
        | Logout

    /// A helper function to create a hyperlink
    let private ( => ) title href =
        A [HRef href] -< [Text title]

    /// A helper function to create a 'fresh' url with a random get parameter
    /// in order to make sure that browsers don't show a cached version.
    let private RandomizeUrl url =
        url + "?d=" + System.Uri.EscapeUriString (System.DateTime.Now.ToString())

    /// User-defined widgets.
    module Widgets =

        /// Widget for displaying login status or a link to login.
        let LoginInfo (ctx: Context<Action>) : list<Content.HtmlElement> =
            let user = UserSession.GetLoggedInUser ()
            [
                (
                    match user with
                    | Some email ->
                        "Log Out (" + email + ")" => 
                            (RandomizeUrl <| ctx.Link Action.Logout)
                    | None ->
                        "Login" => (ctx.Link <| Action.Login None)
                )
            ]

    module Skin =
        open System.Web

        type Page =
            {
                Login : list<Content.HtmlElement>
                Banner : list<Content.HtmlElement>
                Menu : list<Content.HtmlElement>
                Main : list<Content.HtmlElement>
                Sidebar : list<Content.HtmlElement>
                Footer : list<Content.HtmlElement>
                Title : string
            }

        let MainTemplate =
            let path = HttpContext.Current.Server.MapPath("~/Main.html")
            Content.Template<Page>(path)
                .With("title", fun x -> x.Title)
                .With("login", fun x -> x.Login)
                .With("banner", fun x -> x.Banner)
                .With("menu", fun x -> x.Menu)
                .With("main", fun x -> x.Main)
                .With("sidebar", fun x -> x.Sidebar)
                .With("footer", fun x -> x.Footer)

        let WithTemplate title main : Content<Action> =
            Content.WithTemplate MainTemplate <| fun ctx ->
                let menu : list<Content.HtmlElement> =
                    let ( ! ) x = ctx.Link x
                    [
                        "Home" => !Action.Home
                        "Add movie" => !Action.AddMovie
                        //"Protected" => (RandomizeUrl <| !Action.Protected)
                    ]
                    |> List.map (fun link ->
                        Label [Class "menu-item"] -< [link])
                {
                    Login = Widgets.LoginInfo ctx
                    Banner = [H2 [Text title]]
                    Menu = menu
                    Main = main ctx
                    Sidebar = [Text "Put your side bar here"]
                    Footer = [Text "Your website.  Copyright (c) 2011 YourCompany.com"]
                    Title = title
                }

    /// The pages of this website.
    module Pages =

        /// The home page.
        let HomePage : Content<Action> =
            Skin.WithTemplate "Home" <| fun ctx ->
                DB.selectAll()
                |> Array.map (Array.map (fun x -> TD [Div [string x + "." |> Text]]))
                |> Array.map TR
                |> Table
                |> fun x -> [x]

        /// A simple page that echoes a parameter.
        let AddMoviePage : Content<Action> =
            Skin.WithTemplate "Echo" <| fun ctx ->
                [
                    H1 [Text "Contact Form"]
                    Div [
                        new AddMovieControl()]
                ]

        /// A simple login page.
        let LoginPage (redirectAction: option<Action>): Content<Action> =
            Skin.WithTemplate "Login" <| fun ctx ->
                let redirectLink =
                    match redirectAction with
                    | Some action -> action
                    | None        -> Action.Home
                    |> ctx.Link
                [
                    H1 [Text "Login"]
                    P [
                        Text "Login with any username and password='"
                        I [Text "password"]
                        Text "'."
                    ]
                    Div [
                        new LoginControl(redirectLink)
                    ]
                ]

        /// A simple page that users must log in to view.
        let ProtectedPage : Content<Action> =
            Skin.WithTemplate "Protected" <| fun ctx ->
                [
                    H1 [Text "This is protected content - thanks for logging in!"]
                ]

    /// The sitelet that corresponds to the entire site.
    let EntireSite =
        // A sitelet for the protected content that requires users to log in first.
        // A simple sitelet for the home page, available at the root of the application.
        let protect =
            let filter : Sitelet.Filter<Action> =
                {
                    VerifyUser = fun _ -> true
                    LoginRedirect = Some >> Action.Login
                }
            Sitelet.Protect filter
        let home = protect <| Sitelet.Content "/" Action.Home Pages.HomePage
        let authenticated =
            [
                Sitelet.Content "/AddMoviePage" Action.AddMovie Pages.AddMoviePage
                Sitelet.Content "/" Action.Home Pages.HomePage
            ]
            |> List.map protect
            |> Sitelet.Sum

        // An automatically inferred sitelet created for the basic parts of the application.
        let basic =
            Sitelet.Infer <| fun action ->
                match action with
                | Action.Login action->
                    Pages.LoginPage action
                | Action.Logout ->
                    // Logout user and redirect to home
                    UserSession.Logout ()
                    Pages.LoginPage None
                | Action.AddMovie
                | Action.Home ->
                    Content.ServerError

        // Compose the above sitelets into a larger one.
        [
            home
            authenticated
            basic
        ]
        |> Sitelet.Sum

/// Expose the main sitelet so that it can be served.
/// This needs an IWebsite type and an assembly level annotation.
type SampleWebsite() =
    interface IWebsite<SampleSite.Action> with
        member this.Sitelet = SampleSite.EntireSite
        member this.Actions = []

[<assembly: WebsiteAttribute(typeof<SampleWebsite>)>]
do ()
