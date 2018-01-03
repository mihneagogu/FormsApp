import csv
import string
with open("companylist.csv") as csvfile:
    readCSV = csv.reader(csvfile, delimiter=',')
    csharpList = "List<Auction> lstCompanies = new List<Auction>(); \n"
    with open("csfile.txt", "w") as text_file: 
        for row in readCSV:
            result = row[3].find("B") 
            if result != -1:
                csharpList += "lstCompanies.Add(new Auction(%s, %s, %s, %s, %s, %s, %s, %s)); \n" % (row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7])
        print(f"{csharpList}", file=text_file)