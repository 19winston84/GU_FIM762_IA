import csv

input_file = "10highestAndLowestWords.csv"
output_file = "final10.csv"

with open(input_file, 'r') as file:
    reader = csv.DictReader(file)
    data = [row for row in reader if row['Epoch'] == 'Epoch 198']

with open(output_file, 'w', newline='') as file:
    writer = csv.DictWriter(file, fieldnames=reader.fieldnames)
    writer.writeheader()
    writer.writerows(data)