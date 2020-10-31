# HDRP Lens Flare

Lens Flare Asset and Lens Flare Renderer compatible with HDRP.

This is an adaptation of the Lens Flares from the [Fontainebleau Demo](https://github.com/Unity-Technologies/FontainebleauDemo).

## How to use
The Package Manager is a work in progress for Unity. Because of that, your package needs to meet these criteria to become an official Unity package:
- In the project view
  - Right click / create / rendering / Lens Flare
  - Configure your Lens Flare Asset
  - Pick the material you want in the package (additive, alpha, premultiplied)
  - Pick a texture with alpha
- Create a gameobject and add a HDRPLensFlareRenderer component to it
  - Reference the lens flare you want to use

## Disclaimer

Still work in progress. I have seen some issues with distance fade. Don't hesitate to report issues if you find some.
