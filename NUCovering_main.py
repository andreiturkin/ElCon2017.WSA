from CoveringTree import CoveringTree
import timeit

def KitePlotting(iDelta, ShowResOnly=False):
    
    cTree = CoveringTree(5, [8, 12], [8, 12], iDelta)
       
    t = timeit.Timer(lambda: cTree.getCovering(maxLevels))
    exectime = t.timeit(number=1)
    print('Execution time: {}'.format(exectime))
    
    h, props = cTree.getHausdorffDistance()

    cTree.saveRectData_json()
    cTree.saveRectData_csv()

    print('The Hausdorff distance is {}'.format(h))
    if ShowResOnly:
        cTree.saveDistance(props, './Images/Kite__5_8-12_8-12_{}_res.jpeg'.format(iDelta),
                           ZoomIn=True, Grayscale=False, ResOnly=ShowResOnly)
    else:
        cTree.saveDistance(props, './Images/Kite__5_8-12_8-12_{}_cov.jpeg'.format(iDelta),
                           ZoomIn=True, Grayscale=False, ResOnly=ShowResOnly)


def GenerateRectanglesData(iDelta, json):
    cTree = CoveringTree(5, [8, 12], [8, 12], iDelta)
    t = timeit.Timer(lambda: cTree.getCovering(maxLevels))
    exectime = t.timeit(number=1)
    print('Execution time: {}'.format(exectime))
    if json:
        cTree.saveRectData_json("./RectData/rectangle_data_d={}".format(iDelta))
    else:
        cTree.saveRectData_csv("./RectData/rectangle_data_d={}".format(iDelta))

maxLevels = 64


#KitePlotting(0.5, ShowResOnly=False)
#KitePlotting(0.25, ShowResOnly=False)
#KitePlotting(0.07, ShowResOnly=False)

GenerateRectanglesData(0.01, json=True)
GenerateRectanglesData(0.001, json=True)