namespace WebSharper.MediaSession

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    module Enum = 
        let PlaybackState =
            Pattern.EnumStrings "PlaybackState" [
                "none"; "paused"; "playing"
            ]

    let ArtWork = 
        Pattern.Config "ArtWork" {
            Required = []
            Optional = [
                "src", T<string>
                "sizes", T<string>
                "type", T<string>
            ]
        }

    let ChapterInformation =
        Class "ChapterInformation"
        |+> Instance [
            "artwork" =? !| ArtWork 
            "startTime" =? T<float> 
            "title" =? T<string> 
        ]

    let MetadataInit = 
        Pattern.Config "MetadataInit" {
            Required = []
            Optional = [
                "album", T<string>
                "artist", T<string>
                "artwork", !| ArtWork
                "chapterInfo", !| ChapterInformation
                "title", T<string>
            ]
        } 

    let MediaMetadata =
        Class "MediaMetadata"
        |+> Static [
            Constructor (!?MetadataInit?metadata)
        ]
        |+> Instance [
            "album" =@ T<string> 
            "artist" =@ T<string> 
            "artwork" =@ !| ArtWork 
            "chapterInfo" =? !| ChapterInformation 
            "title" =@ T<string> 
        ]

    let MediaSessionActionDetails =
        Pattern.Config "MediaSessionActionDetails" {
            Required = [    ]
            Optional = [
                "action", T<string>
                "fastSeek", T<bool>
                "seekOffset", T<float>
                "seekTime", T<float>
            ]
        }

    let MediaPositionState =
        Pattern.Config "MediaPositionState" {
            Required = []
            Optional = [
                "duration", T<float>
                "playbackRate", T<float>
                "position", T<float>
            ]
        }

    let MediaSession =
        Class "MediaSession"
        |+> Instance [
            "metadata" =@ MediaMetadata 
            "playbackState" =@ Enum.PlaybackState 
            
            "setActionHandler" => T<string>?``type`` * (MediaSessionActionDetails ^-> T<unit>)?callback ^-> T<unit>
            "setCameraActive" => T<bool>?active ^-> T<unit>
            "setMicrophoneActive" => T<bool>?active ^-> T<unit>
            "setPositionState" => !?MediaPositionState?stateDict ^-> T<unit> 
        ]

    let Navigator = 
        Class "Navigator"
        |+> Instance [
            "mediaSession" =? MediaSession
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.MediaSession" [
                Navigator
                MediaSession
                MediaPositionState
                MediaSessionActionDetails
                MediaMetadata
                MetadataInit
                ChapterInformation
                ArtWork
                Enum.PlaybackState
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
