def testApp() {
    echo "Testing the .net app"
}

def buildApp() {
    echo "Building the .net app"
}

def deployApp() {
    echo "deploying the .net app..."
    echo "deploying the .net app version ${params.VERSION}"
}

return this