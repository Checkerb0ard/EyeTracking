# Eye Tracking

Adds eye tracking support to the game. READ THE README

## Features
- Eye Tracking across multiple data sources (Unity Eye Gaze, VRCFaceTracking)
- Support for modders to add their own eye tracking data sources
- Blinking support (Either on most base game avatars, or via added support by the avatar creator)

## Plans
- Foveated rendering support (maybe)

## Installation
PLEASE PLEASE PLEASE make sure you are on [THIS](https://nightly.link/benaclejames/VRCFaceTracking/workflows/publish/master/publish-folder.zip) specific version of VRCFaceTracking.
- Open VRCFaceTracking and install the module that belongs to your headset.
- Download the latest release of this mod.
- Place the downloaded mod in your MelonLoader Mods folder.
- Launch BONELAB and your eyes should be getting tracked if you check a mirror.
Make sure you always launch VRCFaceTracking before launching BONELAB.

## Changelog
- 1.0.1 - Added fusion sync support and released eye tracking SDK.
- 1.0.2 - New SDK components that expose eye tracking data for use in the SDK.

## Other
The mod has with two default eye tracking data sources:
- Unity Eye Gaze: Pulls from Unity's builtin tracking data. Untested and no support is guaranteed.
- OSC/VRCFaceTracking: Pulls from VRCFaceTracking's OSC data. This is the recommended source. Should work with any headset that supports VRCFaceTracking.

## For Modders
Custom components for blinking support will be available soon.
Eye tracking will NOT work if your avatar uses an eye center override. This is not something SLZ supported when implementing their eye gaze system.

If you encounter any issues, ping me in the [BONELAB Discord](https://discord.gg/bonelab) ``checkerboard``