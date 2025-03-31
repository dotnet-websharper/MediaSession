namespace WebSharper.MediaSession

open WebSharper
open WebSharper.JavaScript

[<JavaScript; AutoOpen>]
module Extensions =

    type Navigator with
        [<Inline "$this.mediaSession">]
        member this.MediaSession with get(): MediaSession = X<MediaSession>
