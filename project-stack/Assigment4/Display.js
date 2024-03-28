class Node {
    constructor(data, next = null) {
        this.data = data;
        this.next = next;
    }
}

class SLL {
    constructor() {
        this.head = null;
    }

    addFront(data) {
        const newNode = new Node(data);
        newNode.next = this.head;
        this.head = newNode;
    }

    display() {
        let currentNode = this.head;
        let result = '';

        while (currentNode) {
            result += currentNode.data;
            if (currentNode.next !== null) {
                result += ', ';
            }
            currentNode = currentNode.next;
        }

        return result;
    }
}

// Example usage:
const SLL1 = new SLL();
SLL1.addFront(76);
console.log(SLL1.display()); 

SLL1.addFront(2);
console.log(SLL1.display()); 

SLL1.addFront(11.41);
console.log(SLL1.display()); 
