document.writeln('Hello, world!');
var MYAPP = {};
MYAPP.stooge = {
    "first-name": "Joe",
    "last-name": "Howard"
    };
MYAPP.flight = {
    airline: "Oceanic",
    number: 815,
    departure: {
        IATA: "SYD",
        time: "2004-09-22 14:55",
        city: "Sydney"
    },
    arrival: {
        IATA: "LAX",
        time: "2004-09-23 10:42",
        city: "Los Angeles"
    }
};
var myObject = {
    value: 0,
    increment: function (inc) {
        this.value += typeof inc === 'number' ? inc : 1;
        return this.value;
    }
};

myObject.increment( );
document.writeln(myObject.value); // 1
myObject.increment(2);
document.writeln("myObject.value = " + myObject.value);

document.writeln(myObject.value);
document.writeln(MYAPP.flight.airline);
document.writeln(MYAPP.stooge);

console.log("Como imprimir algo en la consola");

document.writeln("I am");

var x = "document.writeln('Alive!')";
eval(x);

var add = function (a, b) {
    return a + b;
};

var sum = add(3,4);
document.writeln(sum);

myObject.double = function ( ) {
    var that = this; // Workaround.
    var helper = function ( ) {
        that.value = add(that.value, that.value);
    };
    helper( ); // Invoke helper as a function.
};
    // Invoke double as a method.
myObject.double( );
document.writeln(myObject.value);

// Create a constructor function called Quo.
// It makes an object with a status property.
var Quo = function (string) {
    this.status = string;
};
// Give all instances of Quo a public method
// called get_status.
Quo.prototype.get_status = function ( ) {
    return this.status;
};
Quo.prototype.set_status = function (string) {
    this.status = string;
};
// Make an instance of Quo.
var myQuo = new Quo("confused --");
document.writeln(myQuo.get_status( )); // confused pag 30

myQuo.set_status("tragedia");
document.writeln(myQuo.get_status( )); // tragedia

// Make an array of 2 numbers and add them.
var array = [3, 4];
document.writeln(array);
var sum = add.apply("sumaaa", array); // sum is 7
document.writeln(sum);
// Make an object with a status member.
var statusObject = {
status: 'A-OK'
};
document.writeln(statusObject.status);
// statusObject does not inherit from Quo.prototype,
// but we can invoke the get_status method on
// statusObject even though statusObject does not have
// a get_status method.
var status1 = Quo.prototype.get_status.apply(statusObject);
document.writeln(status1);
// status is 'A-OK'
// Make a function that adds a lot of stuff.
// Note that defining the variable sum inside of
// the function does not interfere with the sum
// defined outside of the function. The function
// only sees the inner one.
var sum = function ( ) {
    var i, sum = 0;
    for (i = 0; i < arguments.length; i += 1) {
        sum += arguments[i];
    }
    return sum;
};
document.writeln(sum(4, 8, 15, 16, 23, 42)); // 108

var add = function (a, b) {
    if (typeof a !== 'number' || typeof b !== 'number') {
        throw {
            name: 'TypeError',
        message: 'add needs numbers'
        };
    }
    return a + b;
}

// Make a try_it function that calls the new add
// function incorrectly.
var try_it = function ( ) {
    try {
        add("seven");
    } catch (e) {
        document.writeln(e.name + ': ' + e.message);
    }
}

try_it();

var try_it_good = function ( ) {
    try {
        document.writeln(add(5,15));
    } catch (e) {
        document.writeln(e.name + ': ' + e.message);
    }
}

try_it_good( );

// Add a method conditionally.
Function.prototype.method = function (name, func) {
    if (!this.prototype[name]) {
        this.prototype[name] = func;
        return this;
    }
};

Number.method('integer', function ( ) {
    return Math[this < 0 ? 'ceil' : 'floor'](this);
});
document.writeln((-10 / 3).integer( )); // -3

String.method('trim', function ( ) {
    return this.replace(/^\s+|\s+$/g, '');
});
document.writeln('"' + " neat ".trim( ) + '"');

var hanoi = function hanoi(disc, src, aux, dst) {
    if (disc > 0) {
        hanoi(disc - 1, src, dst, aux);
        document.writeln('Move disc ' + disc +
            ' from ' + src + ' to ' + dst);
        hanoi(disc - 1, aux, src, dst);
    }
};
hanoi(5, 'Src', 'Aux', 'Dst');

// Make a factorial function with tail pag 35
// recursion. It is tail recursive because
// it returns the result of calling itself.
// JavaScript does not currently optimize this form.
var factorial = function factorial(i, a) {
    a = a || 1;
    if (i < 2) {
        return a;
    }
    return factorial(i - 1, a * i);
};
document.writeln(factorial(4)); // 24

var a = 50;
document.writeln("a = " + a);
var foo = function ( ) {
    var a = 3, b = 5;
    document.writeln("a = " + a + ",b =  " + b);
    var bar = function ( ) {
        var b = 7, c = 11;
        document.writeln("b = " + b + ",c =  " + c);
        // At this point, a is 3, b is 7, and c is 11
        a += b + c;
        document.writeln("a = " + a);
        // At this point, a is 21, b is 7, and c is 11
    };
    // At this point, a is 3, b is 5, and c is not defined
    bar( );    
    document.writeln("a = " + a + ",b =  " + b);
    // At this point, a is 21, b is 5
};

foo();
document.writeln("a = " + a);

var myObject = function (valorInicial) {
    var value = valorInicial;
    return {
        initialize: function(value_initial) {
            value = value_initial;
        },
        increment: function (inc) {
            document.writeln(" 1 " + value);
            document.writeln("typeof " + typeof inc);
            value += typeof inc === 'number' ? inc : 1;
            document.writeln(" 2 " + value);
        },
        getValue: function ( ) {
            return value;
        }
    };
}(20);

document.writeln(" 3 " + myObject.getValue());
myObject.increment(5);
document.writeln(" 5 " + myObject.getValue());

myObject.initialize(11);
document.writeln(" 3 " + myObject.getValue());
myObject.increment(5);
document.writeln(" 5 " + myObject.getValue());


// Dining Philosophers problem

function Phil(me, left, right) {
    rendezvous(left, function() {
        console.log(me, ' picked left fork');
        rendezvous(right, function() {
            console.log(me, ' picked right fork');
            rendezvous(left, function() {
                console.log(me, ' dropped left fork');
                rendezvous(right, function() {
                    console.log(me, ' dropped right fork');
                    setTimeout(function() { Phil(me, left, right) }, 0);
                });
            });
        });
    });
}

function Fork(me, left, right) {
    rendezvous(
        left, function() {
            rendezvous(left, function() {
                setTimeout(function() { Fork(me, left, right) }, 0);
            });
        },
        right, function() {
            rendezvous(right, function() {
                setTimeout(function() { Fork(me, left, right) }, 0);
            });
        }
    );
}

function rendezvous() {
    var args = arguments;
    var request = [];
    for (var i = 0; i < args.length / 2; i++) {
        request.push( { chan: args[i*2], func: args[i*2+1] } );
    }
    
    for (var i = 0; i < request.length; i++) {
        request[i].chan.add(request);
    }

    for (var i = 0; i < request.length; i++) {
        if (request[i].chan.match())
            break;
    }
}

function channel() {
    var self = this;
    var requests = [];
    
    self.add = function(req) {
        requests.push(req);
    }

    self.remove = function(req) {
        for (var i = 0; i < requests.length; i++) {
            if (requests[i] == req) {
                requests.splice(i, 1);
                break;
            }
        }
    }
    
    self.match = function() {
        if (requests.length != 2)
            return false;

        // copy array since it will be modified by 'remove' method call
        var reqs = requests.slice(0);
        var funcs = [];
        
        for (var i = 0; i < 2; i++) {
            var req = reqs[i];
            for (var j = 0; j < req.length; j++) {
                if (req[j].chan == self) {
                    funcs.push(req[j].func);
                    break;
                }
            }
            
            for (var j = 0; j < req.length; j++) {
                req[j].chan.remove(req);
            }
        }
        
        // assert(funcs.length == 2);
        // assert(requests.length == 0);
        
        for (var i = 0; i < funcs.length; i++) {
            funcs[i]();
        }
    }
}

var philToLeftFork = [];
var philToRightFork = [];

var N = 5;
document.writeln("Empezamos N = " + N);
// Para probarlo se necesita descomentar
/* for (var i = 0; i < N; i++) {
    philToLeftFork.push(new channel());
    philToRightFork.push(new channel());
}

for (var i = 0; i < N; i++) {
    var j = i;
    setTimeout(function() {
        Phil(j, philToRightFork[(j+1)%N], philToLeftFork[j]) }, 0);
    document.writeln(j);
    setTimeout(function() {
        Fork(j, philToLeftFork[j], philToRightFork[j]) }, 0);
} */

sayHello();
console.log('name', name);
function sayHello() {
    name = 'Shruti Kapoor';
    console.log('My name is ', name);
}

var User;
(function() {
    var instance;
    console.log("empezamos function bebe");
    User = function User() {
        if (instance) {
            console.log("A");
            return instance;
        }
        instance = this;
        // all the functionality
        this.firstName = 'John';
        this.lastName = 'Doe';
        console.log("First Name " + User.firstName);
        return instance;
    };
}());
var user = new User;
document.writeln(user.firstName);

var mySingleton = (function () {

    // Instance stores a reference to the Singleton
    var instance;

    function init() {

        // Singleton

        // Private methods and variables
        function privateMethod(){
            console.log( "I am private" );
        }

        var privateVariable = "Im also private";

        return {

            // Public methods and variables
            publicMethod: function () {
                console.log( "The public can see me!" );
            },

            publicProperty: "I am also public"
        };

    };

    return {

        // Get the Singleton instance if one exists
        // or create one if it doesn't
        getInstance: function () {

        if ( !instance ) {
            instance = init();
        }

        return instance;
        }

    };
})();
  
// Usage:

var singleA = mySingleton.getInstance();
var singleB = mySingleton.getInstance();
console.log( singleA === singleB ); // true

var Car = (function() {

    var _ = PrivateParts.createKey();
  
    function Car(mileage) {
      // Store the mileage property privately.
      _(this).mileage = mileage;
    }
  
    Car.prototype.drive = function(miles) {
      if (typeof miles == 'number' && miles > 0) {
        _(this).mileage += miles;
      } else {
        throw new Error('drive only accepts positive numbers');
      }
    }
  
    Car.prototype.readMileage = function() {
      return _(this).mileage;
    }
  
    return Car;
}());

Car.drive(100);
document.writeln(Car.readMileage());

console.log("fin");