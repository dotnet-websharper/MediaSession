namespace WebSharper.MediaSession.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Notation
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.MediaSession

[<JavaScript>]
module Client =
    // The template is loaded from the DOM, so editing index.html and refreshing
    // the browser is enough; a rebuild is only needed when holes change.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let status = Var.Create "Media Session is not set up yet."
    let audio = JS.Document.GetElementById("audio") |> As<HTMLAudioElement>

    let setupMediaSession () =
        let navigator = JS.Window.Navigator

        if not (isNull navigator.MediaSession) then
            let artwork =
                [|
                    ArtWork(Src = "https://via.placeholder.com/96", Sizes = "96x96", Type = "image/png")
                    ArtWork(Src = "https://via.placeholder.com/128", Sizes = "128x128", Type = "image/png")
                |]

            let metadata =
                new MediaMetadata(MetadataInit(
                    Title = "SoundHelix Song",
                    Artist = "Composer Name",
                    Album = "Sample Album",
                    Artwork = artwork
                ))

            navigator.MediaSession.Metadata <- metadata

            navigator.MediaSession.SetActionHandler("play", fun (_: MediaSessionActionDetails) ->
                audio.Play() |> ignore
                navigator.MediaSession.PlaybackState <- PlaybackState.Playing
            )

            navigator.MediaSession.SetActionHandler("pause", fun (_: MediaSessionActionDetails) ->
                audio.Pause() |> ignore
                navigator.MediaSession.PlaybackState <- PlaybackState.Paused
            )

            navigator.MediaSession.SetActionHandler("nexttrack", fun (_: MediaSessionActionDetails) ->
                audio.CurrentTime <- 0
                audio.Play() |> ignore
            )

            navigator.MediaSession.SetActionHandler("previoustrack", fun (_: MediaSessionActionDetails) ->
                audio.CurrentTime <- 0
                audio.Play() |> ignore
            )

            status := "Media Session metadata and action handlers are set."
        else
            status := "The Media Session API is not available in this browser."

    [<SPAEntryPoint>]
    let Main () =
        IndexTemplate()
            .PageInit(fun () -> setupMediaSession ())
            .status(status.V)
            .Bind()
