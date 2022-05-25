function BinaryHeap(type){   //Constructor for the binary heap tree (min-heap tree). 
	this.type = type.toLowerCase(); //Determines what type of binary heap tree this is (max or min heap tree).  
	
	this.array = []; 
	
	this.HeapifyUp = function(node){ //When inserting we must re-heap the tree to maintain its heap property.  

		this.array.push(node); //Adds the new array to the bottom level of the heap.
  
	  var i = this.array.length-1; //Gets the last index of the array. 
  
	  var finished = false; 
  
	  if(this.type === "max"){ //Case for max-heap
	  
		  while(!finished){		
			  if(this.array[i] > this.array[Math.floor((i-1)/2)]){ 
				  var tmp = this.array[Math.floor((i-1)/2)];
			  
				  this.array[Math.floor((i-1)/2)] = this.array[i]; 
			  
				  this.array[i] = tmp; 
			  
				  i = Math.floor((i-1)/2); 
			  }
			  else{ 
				  finished = true;
			  }
		  }
	  }
  
	  if(this.type === "min"){ //Case for min-heap
		  while(!finished){
			  if(this.array[i] < this.array[Math.floor((i-1)/2)]){ 
				  var tmp = this.array[Math.floor((i-1)/2)];
				  this.array[Math.floor((i-1)/2)] = this.array[i]; 
				  this.array[i] = tmp; 
			  
				  i = Math.floor((i-1)/2); 
			  }
			  
			  else{ 
				  finished = true;
			  }
		  }
	  }
	};

	this.HeapifyDown = function(){

	  var end = this.array.length-1; 
	  
	  var i = 0; 
	  
	  this.array[i] = this.array[end]; //Replaces the root with the last element on the last level of the tree. 
	  
	  this.array.pop(); //Deletes the last level node of the tree.  
	  
	  var finished = false; 

	  if(this.type === "max"){
		  while(!finished){
			  if(this.array[i] < this.array[2*i+1] || this.array[2*i+2]){ //If the root is smaller than any of its children
		   
					if(this.array[2*i+1] && this.array[2*i+2] != undefined)
			  
				  var largestChildIndex = (this.array[2*i+1] >= this.array[2*i+2]) ? (2*i)+1 : (2*i)+2; //If the left child is bigger or equal to the right child return the index of the left child otherwise return index of the right child. 
				
					else if(this.array[2*i+1] != undefined){
					  var largestChildIndex = (2*i)+1; 
					}
					else{
						var largestChildIndex = (2*i)+2; 
					}
		  
					var tmp = this.array[largestChildIndex]; 
			  
				  this.array[largestChildIndex] = this.array[i]; 
			  
				  this.array[i] = tmp;
			  
				  i = largestChildIndex; 	
			  }

			  else{
				   finished = true; 
			  }
		   }
		}
	
	  else if(this.type === "min"){
		  while(!finished){
			  if(this.array[i] > this.array[2*i+1] || this.array[2*i+2]){ 
			  
				  if(this.array[2*i+1] && this.array[2*i+2] != undefined)
					  var smallestChildIndex = (this.array[2*i+1] <= this.array[2*i+2]) ? (2*i)+1:(2*i)+2; 
			  
				  else if(this.array[2*i+1] != undefined) 
					  var smallestChildIndex = (2*i)+1;
			  
				  else
						 var smallestChildIndex = (2*i)+2; 

				  var tmp = this.array[smallestChildIndex]; 
			  
				  this.array[smallestChildIndex] = this.array[i]; 
			  
				  this.array[i] = tmp;
			  
				  i = smallestChildIndex; 
			  }

			  else{
				   finished = true; 
			  }
		  }
	  }
	};

  this.insert = function(node){
		this.HeapifyUp(node);
	};

	this.showBinaryHeap = function(){
		console.log(this.array);
	};

	this.remove = function(){
		this.HeapifyDown();
   };

   this.get = function(index){ 
	   return this.array[index];
   }
}


$(document).ready(function(){ 	//When the document is read or completely loaded. 
	
	var c = $('canvas')[0];	 
	var ctx = c.getContext("2d");
	var heap; 
	$('canvas').attr({
		'width': $('.myCanvas').outerWidth(),
		'height':$('.myCanvas').outerHeight()
	});

	$('.add-button').click(function(){ 

		var inputValue=$('.contentInput').val();
		if(inputValue!=''){	
			var item = inputValue;
			heap.insert(parseInt(item));
			heap.showBinaryHeap();
			drawTree(c.width/2,25,0,0)
		}
	});
	
	$('.remove-button').on("click",function(){ 
		heap.remove();
		ctx.clearRect(0,0, $('.myCanvas').outerWidth(), $('.myCanvas').outerHeight());
		drawTree(c.width/2,25,0,0);
	});

	$(".submit-button").on("click",function(){ 

		if($('.infoInput').val().toLowerCase() === 'min' || $('.infoInput').val().toLowerCase() === 'max'){
			$(".infoContent-block").css("display", "block");
			$(".infoType-block").css("display","none");
	
			heap = new BinaryHeap($('.infoInput').val());
		}
	});

	$(".infoContent-block .add-button").on("click",function(){
		$(".infoContent-block input").val("");
	});
	
	$(".infoContent-block .remove-button").on("click",function(){
		$(".infoContent-block input").val(""); 
	});

	$(".restart-button").on("click",function(){ 
		$(".infoContent-block input").val(""); 
		$(".infoContent-block").css("display","none");
		
		$(".infoType-block input").val("");  
		$(".infoType-block").css("display","block");
		
		$('canvas').attr({   //Sets the canvas element with the dimensions of the parent div. 
			'width': 1335,
			'height': 400
		});
		
		$('.myCanvas').outerHeight(400);
		$('.myCanvas').outerWidth(1335);
	});

	function drawTree(x1,y1,position,level){	//main function for drawing the Binary Heap Tree onto the canvas.
		var distances_between_nodes_on_lvl = [250,125,62.5,31.25,15.625,7.8125,3.90625,1.953125]; //Used to space the nodes.

		if(heap.get(position) != null){
			var l_x2 = x1-distances_between_nodes_on_lvl[level];	//Sets the x position of left child.
			var r_x2 = x1+distances_between_nodes_on_lvl[level];	//Sets the x position of right child.
			var y2 = y1+25; 	//Sets height of the next child.

			if(heap.get(2*position+1) != null){	//If the left child is there
				drawConnection(x1, y1,l_x2,y2);	//Connect from parent the left child.
				drawTree(l_x2,y2,2*position+1,level+1);	//Tranverses the left tree
			}

			if(heap.get(2*position+2) != null){	//If the right child is there
				drawConnection(x1, y1,r_x2,y2);	//Connect with parent the right child.
				drawTree(r_x2,y2,2*position+2,level+1);	//Tranverses the right tree.
			}
			
			var currentNode = heap.get(position);	//Get the current node and draw it.
			drawNode(currentNode, x1, y1);	//Draw node.
		}							
	}

	function drawConnection(x1, y1, x2, y2){ 
		ctx.strokeStyle = 'black';
		ctx.lineWidth = 2;
		ctx.beginPath();
		ctx.moveTo(x1-15,y1);
		ctx.lineTo(x2+15,y2);
		ctx.closePath();
		ctx.stroke();
	}

	function drawNode(content, posX, posY){	
		ctx.beginPath();
		ctx.lineWidth = 5;
		ctx.arc(posX,posY,20,0,2*Math.PI);
		ctx.fillStyle = '#c3e7ff'; 
		ctx.fill(); 
		ctx.lineWidth = 5;
		ctx.font = '17px Arial';
		ctx.fillStyle = 'black';
		ctx.textAlign = 'center';
		ctx.fillText(content,posX, posY+4);
	}
});