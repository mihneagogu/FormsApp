import csv
import string
with open("companylist.csv") as csvfile:
    readCSV = csv.reader(csvfile, delimiter=',')
    csharpList = "";
    with open("csfile.txt", "w") as text_file: 
        for row in readCSV:
            result = row[3].find("B") 
            if result != -1:
                csharpList += "companies.Add(new PyAuction(\"%s\",  \"%s\", \"%s\", \"%s\", \"%s\", \"%s\", \"%s\", \"%s\")); \n" % (row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7])
        text_file.write(csharpList)