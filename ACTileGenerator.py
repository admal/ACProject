from sets import Set

# Create Tile Rectangle of size 3 and more (create first 30-something tiles)
# and longer size is not greater than 10.
def createRectangeTile(width, height):
    if(height > 10 or width > 10 or (width, height) in pairs):
        return ''
    else:
        pairs.add((width, height))
        pairs.add((height, width))
        tile = str(width) + " " + str(height)

        for i in range(height):
            tile += "\n"
            for j in range(width):
                tile += "1"
                if j < width-1:
                    tile += " "
        tile += "\n"

    return tile;

#  Create tile that when rotated by 180 degree will remain the same,
#  which area is smaller or equal 10 and all dimensions are not greater than 8.
# mid - tells how many times the rigid middle should be repeated
def createWeirdTile(width, mod, offset, mid):
    tileData = str(width) + " " + str(mid + 2) + "\n"
    tileO = ''
    for i in range(width): # first part of object { 1 0 1 0 1} for mod=2, offset=0
        if(i%mod == offset):
            tileO += "1"
        else:
            tileO += "0"
        if i < width-1:
            tileO += " "
    tileO += "\n"

    tileM = ''
    for m in range(mid): # middle rigid part
        for i in range(width):
            tileM += "1"
            if i < width-1:
                tileM += " "
        tileM += "\n"

    tile = tileData + tileO + tileM + tileO
    return tile


############### PROGRAM ###################
filename = 'tiles.txt'
file = open(filename, 'w')
file.truncate()
pairs = Set([(1,1)]) # this set is just to make sure that rectangle tiles won't be repeated

tiles = Set()
for i in range(3, 11):
    for j in range(i, 11):
        tile = createRectangeTile(i,j)
        tiles.add(tile)


tiles.add(createWeirdTile(2, 2, 0, 1)) #width, mod, offset, mid 1
tiles.add(createWeirdTile(3, 2, 0, 1))
tiles.add(createWeirdTile(4, 2, 0, 1))
tiles.add(createWeirdTile(5, 2, 0, 1))
tiles.add(createWeirdTile(2, 2, 1, 1)) #width, mod, offset, mid 2
tiles.add(createWeirdTile(3, 2, 1, 1))
tiles.add(createWeirdTile(4, 2, 1, 1))
tiles.add(createWeirdTile(5, 2, 1, 1))
tiles.add(createWeirdTile(2, 2, 2, 2)) #width, mod, offset, mid 3
tiles.add(createWeirdTile(3, 2, 2, 1))
tiles.add(createWeirdTile(4, 2, 2, 1))
tiles.add(createWeirdTile(5, 2, 2, 1))
tiles.add(createWeirdTile(6, 4, 0, 1)) #width, mod, offset, mid 4
tiles.add(createWeirdTile(3, 3, 0, 1))
tiles.add(createWeirdTile(4, 3, 0, 1))
tiles.add(createWeirdTile(5, 3, 0, 1))
tiles.add(createWeirdTile(8, 5, 4, 1)) #width, mod, offset, mid 5
tiles.add(createWeirdTile(2, 2, 0, 3))
tiles.add(createWeirdTile(4, 3, 1, 1))
tiles.add(createWeirdTile(5, 3, 1, 1))
tiles.add(createWeirdTile(2, 2, 0, 2)) #width, mod, offset, mid 6
tiles.add(createWeirdTile(3, 2, 0, 2))
tiles.add(createWeirdTile(4, 4, 0, 1))
tiles.add(createWeirdTile(5, 4, 0, 1))

file.write("80 " + str(len(tiles)) + "\n")
for tile in tiles:
    file.write(tile)

print len(tiles)
file.close()
