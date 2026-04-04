# Eye Tracking

Adds eye and face tracking support to the game.

## Features
- Eye Tracking across multiple data sources (VRCFaceTracking, Babble)
- Support for modders to add their own eye tracking data sources

## Plans
- Foveated rendering support (maybe)

## Installation (VRCFT)
PLEASE PLEASE PLEASE make sure you are on [THIS](https://nightly.link/benaclejames/VRCFaceTracking/workflows/publish/master/publish-folder.zip) specific version of VRCFaceTracking.
- Open VRCFaceTracking and install the module that belongs to your headset.
- Download the latest release of this mod.
- Place the downloaded mod in your MelonLoader Mods folder.
- Launch BONELAB.
Make sure you always launch VRCFaceTracking before launching BONELAB.

## Installation (Babble)
- Open Baballonia and setup like normal.
- Download the latest release of this mod.
- Place the downloaded mod in your MelonLoader Mods folder.
- Launch BONELAB.
Make sure you always launch Baballonia before launching BONELAB.

## Changelog
- 1.0.1 - Added fusion sync support and released eye tracking SDK.
- 1.0.2 - New SDK components that expose eye tracking data for use in the SDK.
- 1.1.0 - Almost complete rewrite of the mod. Fixed Fusion joining.

## Other
The mod has with two default eye tracking data sources:
- VRCFaceTracking: Pulls from VRCFaceTracking's OSC data. This is the recommended source. Should work with any headset that supports VRCFaceTracking.
- Babble: Pulls from OSC data provided by Baballonia. Useful if you use Babble and would prefer to not have VRCFT open. Face support will come in a future update. Please use VRCFT with the babble module for face tracking for now.

## For Modders
Scripts for adding blink support and face tracking to avatars are available [here](https://github.com/Checkerb0ard/EyeTracking/tree/main/EyeTracking/MarrowSDK).

## Support
If you encounter any issues, ping me in the [BONELAB Discord](https://discord.gg/bonelab) ``checkerboard``