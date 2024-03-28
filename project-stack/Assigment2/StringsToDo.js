//Remove Blanks : Create a function that, given a string, returns all of that string’s contents, but without blanks

function RemoveBlanks(sentence)
{
    let result = "";
    for (let index = 0; index < sentence.length; index++) {
        if(sentence[index] !== " ")
        {
            result += sentence[index];
        }
        
    }
    
    return result;
}

// Get Digits: Create a JavaScript function that given a string, returns the integer made from the string’s digits

function GetDigits(sentence)
{
    let result = "";
    for (let index = 0; index < sentence.length; index++) {
        if(!isNaN(sentence[index]) && sentence[index] !== " ")
        {
            result += sentence[index];
        }
        
    }
    return Number(result);
}

// Create a function that, given a string, returns the string’s acronym

function Acronyms(sentence)
{
    let result="";
    if(sentence[0] !== " ")
    {
        result += sentence[0].toUpperCase();;
    }
    for (let index = 1; index < sentence.length; index++) {
        if (sentence[index] === " " && sentence[index + 1]) {
            result += sentence[index + 1].toUpperCase();
        }
    
    }
    return result;
}
// Create a function that, given a string, returns the number of non-space characters found in the string. 

function CountNonSpaces(sentence) {
    let count = 0;
    for (let i = 0; i < sentence.length; i++) {
        if (sentence[i] !== ' ') {
            count++;
        }
    }
    return count;
}

// Create a function that, given an array of strings and a numerical value, returns an array that only contains strings > than or = to the given value
function RemoveShorterStrings(sentence, value) {
    var result = [];
    for (var i = 0; i < sentence.length; i++) {
        if (sentence[i].length >= value) {
            result.push(sentence[i]);
        }
    }
    return result;
}

// Testing :

console.log(RemoveBlanks("I can not BELIEVE it's not BUTTER")); 
console.log(GetDigits("0s1a3y5w7h9a2 t4?6!8?0")); 
console.log(Acronyms(" there's no free lunch - gotta pay yer way. "));
console.log(CountNonSpaces("Honey pie, you are driving me crazy"));
console.log(RemoveShorterStrings(['There', 'is', 'a', 'bug', 'in', 'the', 'system'], 3));