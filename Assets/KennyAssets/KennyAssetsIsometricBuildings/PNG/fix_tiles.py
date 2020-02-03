import os 
from PIL import Image
# apply mask to images

files = os.listdir('.')
png_files = []

masks = [Image.open(f) for f in files if 'mask' in f]

for f in files:
    if f.endswith('.png') and 'mask' not in f:
        im = Image.open(f)
        for mask in masks:
            if mask.size != im.size:
                continue
            print(f, im.size)
            new_img = Image.new('RGBA', im.size)
            new_img.paste(im, (0, 0), mask)
            new_img.save(f)
            break
