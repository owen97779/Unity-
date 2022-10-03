# This is a sample Python script.
import numpy as np
import math
from tkinter import *


def conic_section(p1, p2, p3, p4, p5):
    x1, y1, x2, y2, x3, y3, x4, y4, x5, y5 = p1[0], p1[1], p2[0], p2[1], p3[0], p3[1], p4[0], p4[1], p5[0], p5[1]
    a = np.array([[x1 * y1, y1 * y1, x1, y1, 1],
                  [x2 * y2, y2 * y2, x2, y2, 1],
                  [x3 * y3, y3 * y3, x3, y3, 1],
                  [x4 * y4, y4 * y4, x4, y4, 1],
                  [x5 * y5, y5 * y5, x5, y5, 1]])
    b = np.array([-x1 * x1, -x2 * x2, -x3 * x3, -x4 * x4, -x5 * x5])
    x = np.linalg.solve(a, b)
    B2, C2, D2, E2, F2 = x[0], x[1], x[2], x[3], x[4]
    A2 = -(B2 * x1 * y1 + C2 * y1 * y1 + D2 * x1 + E2 * y1 + F2) / (x1 * x1)
    return A2, B2, C2, D2, E2, F2


root = Tk()
root.title("Ellipse equation")
xy = ["X", "Y"]
entrylist = []

for x in range(0, 5):
    for y in range(0, 4):
        if y % 2 == 0:
            e = Entry(root, width=25, borderwidth=5)
            e.grid(row=x, column=y, padx=10, pady=10)
            entrylist.append(e)

        if y % 2 == 1:
            t = xy[int((y + 1) / 2) - 1] + str(x + 1)
            label = Label(root, text=t)
            label.grid(row=x, column=y)

button_compute = Button(root, text="Compute equation", command=lambda: compute())
button_compute.grid(row=5, column=0, padx=50, columnspan=4)


def compute():
    points = []
    for i in range(0, len(entrylist), 2):
        px = float(entrylist[i].get()) * (10 ** 3) * (100 / (1.49 * (10 ** 11)))
        py = float(entrylist[i + 1].get()) * (10 ** 3) * (100 / (1.49 * (10 ** 11)))
        point = [px, py]
        points.append(point)

    A, B, C, D, E, F = conic_section(points[0], points[1], points[2], points[3], points[4])
    X_C = (B * E - 2 * C * D) / (4 * A * C - B * B)
    Y_C = (B * D - 2 * A * E) / (4 * A * C - B * B)

    print(str(A) + "x\u00b2+" + str(B) + "xy+" + str(C) + "y\u00b2+" + str(D) + "x+" + str(E) + "y+" + str(F))
    a2 = (2 * (A * X_C * X_C / F + C * Y_C * Y_C / F + B * X_C * Y_C / F - 1)) / (
                A / F + C / F + math.sqrt((A / F - C / F) ** 2 + (B / F) ** 2))
    b2 = (2 * (A * X_C * X_C / F + C * Y_C * Y_C / F + B * X_C * Y_C / F - 1)) / (
                A / F + C / F - math.sqrt((A / F - C / F) ** 2 + (B / F) ** 2))

    semiMajor = math.sqrt(a2)
    semiMinor = math.sqrt(b2)
    print("semi-major = " + str(semiMajor), "semi-minor = "+ str(semiMinor))
    gradient = (-2 * A * points[0][0] - B * points[0][1] - D) / (B * points[0][0] + 2 * C * points[0][1] + E)
    intercept = points[0][1] - gradient * points[0][0]
    print("y=" + str(gradient) + "x" + "+" + str(intercept))
    print(points[0][0], points[0][1])


# ex1 = Entry(root, width=25, borderwidth=5)
# ex1.grid(row=0, column=1, padx=10, pady=10)
# ex1.insert(0,"2.142609633873336")
#
# ex2 = Entry(root, width=25, borderwidth=5)
# ex2.grid(row=0, column=3, padx=10, pady=10)
# ex2.insert(0,"2.142609633873336")
#
# labelx1 = Label(root, text="X1")
# labelx1.grid(row=0,column=0)
#
# labelx2 = Label(root, text="X2")
# labelx2.grid(row=0,column=2)

root.mainloop()

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.


# Mercury
# A, B, C, D, E, F = conic_section([14.3799304287, -43.0723550132], [31.8299241197, -26.0514367793],
#                                  [35.3722871826, 1.33741643238],
#                                  [16.44373044, 26.1629968092], [-16.0876992818, 27.248531649])
#
# print(A, B, C, D, E, F)
