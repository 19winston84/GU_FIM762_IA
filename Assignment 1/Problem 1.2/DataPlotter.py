import matplotlib.pyplot as plt
import pandas as pd

# Read the CSV file
data = pd.read_csv('accuracyData.csv')

# Extract the training and validation accuracies
training_accuracy = data['Training accuracy']
validation_accuracy = data['Validation accuracy']

# Create a line plot
plt.axhline(y=0.95, color='k', linestyle='--', linewidth=0.5)
plt.plot(training_accuracy, label='Training Accuracy')
plt.plot(validation_accuracy, label='Validation Accuracy')

# Add labels and title

plt.xlabel('Epoch')
plt.ylabel('Accuracy')
plt.title('Training and Validation Accuracies')

# Add legend
plt.legend()

# Show the plot
#plt.show()

# Save the plot as an SVG file
plt.savefig('AccuracyPlot.svg')

# Find the highest Validation accuracy
highest_validation_accuracy = validation_accuracy.max()

# Find the corresponding Epoch, Training accuracy, and Test accuracy
highest_validation_epoch = data.loc[data['Validation accuracy'] == highest_validation_accuracy, 'Epoch'].values[0]
highest_validation_training_accuracy = data.loc[data['Validation accuracy'] == highest_validation_accuracy, 'Training accuracy'].values[0]
highest_validation_test_accuracy = data.loc[data['Validation accuracy'] == highest_validation_accuracy, 'Test accuracy'].values[0]

# Print the results
print(f"Highest Validation Accuracy:\nEpoch: {highest_validation_epoch}\nTraining Accuracy: {highest_validation_training_accuracy}\nValidation Accuracy: {highest_validation_accuracy}\nTest Accuracy: {highest_validation_test_accuracy}")