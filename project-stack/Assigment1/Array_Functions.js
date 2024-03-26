// Function to insert a value at the beginning of an array
function PushFront(arr, value) {
    for (let i = arr.length; i > 0; i--) {
        arr[i] = arr[i - 1];
    }
    arr[0] = value;
    return arr;
}

// Function to remove and return the value at the beginning of an array
function PopFront(arr) {
    const removed = arr[0];
    for (let i = 0; i < arr.length - 1; i++) {
        arr[i] = arr[i + 1];
    }
    arr.length--; 
    console.log(removed + " returned, with " + arr + " printed in the function");
    return removed;
}

// Function to insert a value at a specific index in an array
function insertAt(arr, index, value) {
    for (let i = arr.length; i > index; i--) {
        arr[i] = arr[i - 1];
    }
    arr[index] = value;
    return arr;
}

// Function to remove and return the value at a specific index in an array
function removeAt(arr, index) {
    const removed = arr[index];
    for (let i = index; i < arr.length - 1; i++) {
        arr[i] = arr[i + 1];
    }
    arr.length--; 
    console.log(removed + " returned, with " + arr + " printed in the function");
    return removed;
}

// Function to swap positions of successive pairs of values in an array
function SwapPairs(arr) {
    for (let i = 0; i < arr.length - 1; i += 2) {
        const temp = arr[i];
        arr[i] = arr[i + 1];
        arr[i + 1] = temp;
    }
    return arr;
}

// Function to remove duplicate values from a sorted array
function RemoveDuplicates(arr) {
    let writeIndex = 1;
    for (let i = 1; i < arr.length; i++) {
        if (arr[i] !== arr[i - 1]) {
            arr[writeIndex++] = arr[i];
        }
    }
    arr.length = writeIndex;
    return arr;
}

// Testing :
console.log(PushFront([5, 7, 2, 3], 8)); 
console.log(PopFront([0, 5, 10, 15])); 
console.log(insertAt([100, 200, 5], 2, 311)); 
console.log(removeAt([1000, 3, 204, 77], 1)); 
console.log(SwapPairs([1, 2, 3, 4])); 
console.log(RemoveDuplicates([-2, -2, 3.14, 5, 5, 10])); 