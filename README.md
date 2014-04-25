XNA Basic Chunked Tilerenderer
==============================

Basic chunked tile rendering for XNA.

It uses RenderTarget2D for each chunk.

================================================

**VRAM usage :**
* 0.5mb per 256x256 chunk (calculated using (bytes)(256 * 256 * 8))
* **1280x720** uses ~30 chunks = max(15mb of vram)
* **1920x1080** uses ~60 chunks = max(30mb of vram)

***

**Rendering speed(MonoGame) :**
Those tests are relative, post real test results in issues. In XNA it rendered ~2.5 times faster. Tested on Intel HD 4000.
* 16x16 tile and 256x256 chunks on 1280x720 = ~0.5ms per chunk
* 8x8 tile and 128x128 chunks on 1280x720 = ~0.75ms per chunk
