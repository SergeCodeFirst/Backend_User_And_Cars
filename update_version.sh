#!/bin/bash

# Ensure the script exits on any error
set -e

# Function to display usage
usage() {
    echo "Usage: $0 {patch|minor|major}"
    exit 1
}

# Check for the correct number of arguments
if [ "$#" -ne 1 ]; then
    usage
fi

# Specify the project file name
project_file="backend.csproj"

# Read the current version from the .csproj file
current_version=$(grep '<Version>' "$project_file" | sed -e 's/<[^>]*>//g' | xargs)
echo "Current version: $current_version"

# Increment the version based on the argument
case $1 in
    patch)
    new_version=$(echo $current_version | awk -F. -v OFS=. '{$3+=1;print}')
    ;;
    minor)
    new_version=$(echo $current_version | awk -F. -v OFS=. '{$2+=1;$3=0;print}')
    ;;
    major)
    new_version=$(echo $current_version | awk -F. -v OFS=. '{$1+=1;$2=0;$3=0;print}')
    ;;
    *)
    usage
    ;;
esac

echo "New version: $new_version"

# Update the .csproj file with the new version
sed -i -v "s/<Version>.*<\/Version>/<Version>$new_version<\/Version>/" "$project_file"

# # Commit the change
# git add "$project_file"
# git commit -m "Update version to $new_version"
# git push


# How to USE: Run the following command in the terminal base on the number you want to update
# Do not forget to add execute right 

# chmod +x update_version.sh
# ./update_version.sh patch   # For patch increment
# ./update_version.sh minor   # For minor increment
# ./update_version.sh major   # For major increment

