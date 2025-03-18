# WebSharper Media Session API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Media Session API](https://developer.mozilla.org/en-US/docs/Web/API/Media_Session_API), enabling enhanced media playback controls and metadata integration in WebSharper applications.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Media Session API.

2. **Sample Project**:
   - Demonstrates how to use the Media Session API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/MediaSession/)

## Installation

To use this package in your WebSharper project, add the NuGet package:

```bash
   dotnet add package WebSharper.MediaSession
```

## Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/MediaSession.git
   cd MediaSession
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.MediaSession/WebSharper.MediaSession.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.MediaSession.Sample
   dotnet build
   dotnet run
   ```

## Example Usage

Below is an example of how to use the Media Session API in a WebSharper project:

```fsharp
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

    // Function to setup media session metadata and actions
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

            // Define play action
            navigator.MediaSession.SetActionHandler("play", fun (_:MediaSessionActionDetails) ->
                audio.Play()
                navigator.MediaSession.PlaybackState <- PlaybackState.Playing
            )

            // Define pause action
            navigator.MediaSession.SetActionHandler("pause", fun (_:MediaSessionActionDetails) ->
                audio.Pause()
                navigator.MediaSession.PlaybackState <- PlaybackState.Paused
            )

            // Define next track action
            navigator.MediaSession.SetActionHandler("nexttrack", fun (_:MediaSessionActionDetails) ->
                audio.CurrentTime <- 0
                audio.Play()
            )

            // Define previous track action
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
```

## Important Considerations

- **Browser Support**: Ensure your target browser supports the Media Session API.
- **Metadata Display**: Media players and notifications may display metadata differently across platforms.
- **Custom Action Handling**: You can extend action handlers to integrate with your application's media controls.
