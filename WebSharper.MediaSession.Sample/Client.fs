namespace WebSharper.MediaSession.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.MediaSession

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let audio = JS.Document.GetElementById("audio") |> As<HTMLAudioElement>

    let setupMediaSession () =
        let navigator = As<Navigator>(JS.Window.Navigator)

        let artWork1 = ArtWork (
            Src = "https://via.placeholder.com/96",
            Sizes = "96x96",
            Type = "image/png"
        )

        let artWork2 = ArtWork (
            Src = "https://via.placeholder.com/128",
            Sizes = "128x128",
            Type = "image/png"
        )

        if not (isNull navigator?mediaSession) then
            let metadata = new MediaMetadata(MetadataInit(
                Title = "SoundHelix Song",
                Artist = "Composer Name",
                Album = "Sample Album",
                Artwork = [|artWork1; artWork2|]
            ))

            navigator.MediaSession.Metadata <- metadata

            navigator.MediaSession.SetActionHandler("play", fun (_:MediaSessionActionDetails) ->
                audio.Play()
                navigator.MediaSession.PlaybackState <- PlaybackState.Playing
            )

            navigator.MediaSession.SetActionHandler("pause", fun (_:MediaSessionActionDetails) ->
                audio.Pause()
                navigator.MediaSession.PlaybackState <- PlaybackState.Paused
            )

            navigator.MediaSession.SetActionHandler("nexttrack", fun (_:MediaSessionActionDetails) ->
                audio.CurrentTime <- 0
                audio.Play()
            )

            navigator.MediaSession.SetActionHandler("previoustrack", fun (_:MediaSessionActionDetails) ->
                audio.CurrentTime <- 0
                audio.Play()
            )

    [<SPAEntryPoint>]
    let Main () =        

        IndexTemplate.Main()
            .PageInit(fun () -> 
                setupMediaSession()
            )
            .Doc()
        |> Doc.RunById "main"
