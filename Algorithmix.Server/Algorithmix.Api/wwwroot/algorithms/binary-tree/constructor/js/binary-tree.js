const generateUUID = () => {
    let d = new Date().getTime();
    return 'Nxxxxxxxx_xxxx_4xxx_yxxx_xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
        const r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);

        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
};

const drawSettings = {
    pixelOffset: 250,
    nodesToAnimate: [],
    animationSpeed: { speed: 150, delay: 250, noAnimation: false }
};

const treeNode = function (value) {
    this.value = value;
    this.id = generateUUID();
}

treeNode.prototype.leftNode = null;
treeNode.prototype.rightNode = null;
treeNode.prototype.parent = null;
treeNode.prototype.locX = null;
treeNode.prototype.locY = 50;
treeNode.prototype.depth = 0;
treeNode.prototype.inRightBranch = true;
treeNode.prototype.xOffset = 1;
treeNode.prototype.size = 20;
treeNode.prototype.fillStyle = '#b2cfff';
treeNode.prototype.fillStyleText = '#000';

treeNode.prototype.children = function () {
    const children = [];

    if (this.leftNode) children.push(this.leftNode);
    if (this.rightNode) children.push(this.rightNode);

    return children;
};

treeNode.prototype.allChildren = function () {
    const children = [];

    if (this.leftNode) {
        children.push(this.leftNode);
        $.each(this.leftNode.allChildren(), function (index, child) {
            children.push(child);
        })
    }

    if (this.rightNode) {
        children.push(this.rightNode);
        $.each(this.rightNode.allChildren(), function (index, child) {
            children.push(child);
        })
    }

    return children;
}

let rootNode = null;

const treeManager = {
    addNode: (value, numbersToAdd) => {
        const node = new treeNode(value);

        if (!rootNode) {
            rootNode = node;
            node.locX = $('#svg').width() / 2;
        } else {
            const parentNode = treeManager.findParentFromValue(rootNode, value);
            node.depth = parentNode.depth + 1;
            node.parent = parentNode;
            node.balance = 0;
            treeManager.addToParent(node);
        }

        drawSettings.nodesToAnimate.push([{ node, animationType: 'add' }])
        view.animateNodes(treeManager.returnNodesToAnimate(), numbersToAdd);
    },

    addToParent: (node) => {
        if (node.parent) {
            const parentNode = node.parent, pixelOffset = drawSettings.pixelOffset;
            const delta = 1.8;

            if (node.value >= parentNode.value) {
                parentNode.rightNode = node;
                node.locX = parentNode.locX + pixelOffset / Math.pow(delta, node.depth - 1);
            } else {
                parentNode.leftNode = node;
                node.locX = parentNode.locX - pixelOffset / Math.pow(delta, node.depth - 1);
            }

            node.locY = rootNode.locY + pixelOffset / 4 * node.depth;
        }

        const fixCollisions = (nodeToCheck) => {
            const closeness = 2;

            $.each(rootNode.allChildren(), function (index, node) {
                if (
                    nodeToCheck !== node
                    && nodeToCheck.locY === node.locY
                    && (Math.abs(nodeToCheck.locX - node.locX) < closeness * node.size)
                ) {
                    nodeToCheck.locX += Math.sign(nodeToCheck.locX - node.locX) * closeness / 2 * nodeToCheck.size;
                    node.locX += Math.sign(node.locX - nodeToCheck.locX) * closeness / 2 * nodeToCheck.size;

                    const childNodesToAnimate = []

                    childNodesToAnimate.push({
                        node,
                        animationType: 'move',
                        x: node.locX,
                        y: node.locY,
                        lineAnimation: {
                            x1: node.parent.locX - node.locX,
                            y1: node.parent.locY - node.locY, x2: 0, y2: 0
                        }
                    });

                    $.each(node.children(), function (index, node) {
                        childNodesToAnimate.push({
                            node,
                            animationType: 'move',
                            x: node.locX,
                            y: node.locY,
                            lineAnimation: {
                                x1: node.parent.locX - node.locX,
                                y1: node.parent.locY - node.locY, x2: 0, y2: 0
                            }
                        });
                    });

                    if (childNodesToAnimate.length > 0) drawSettings.nodesToAnimate.push(childNodesToAnimate);
                }
            });
        }

        fixCollisions(node);
    },

    balanceAfterAdd: (targetNode, targetBalance) => {
        let node = targetNode;
        let balance = targetBalance;

        while (node != null) {
            node.balance += balance;
            balance = node.balance;

            if (balance === 0) {
                return;
            }
        }
    },

    searchForNode: (value) => {
        let node = rootNode ? treeManager.searchDownTree(rootNode, value) : null;

        if (node && node.value === value) {
            let i = 0;

            while (i < 2) {
                drawSettings.nodesToAnimate.push([{ node, animationType: 'pop' }]);
                i++;
            }

            return node;
        } else {
            drawSettings.nodesToAnimate = [];
            alert('Значение ' + String(value) + ' отсутствует в дереве.');
        }
    },

    searchDownTree: (node, value) => {
        drawSettings.nodesToAnimate.push([{ node, animationType: 'pop' }]);

        if (value === node.value) return node;
        else if (value < node.value) {
            if (node.leftNode == null) return null;
            else return treeManager.searchDownTree(node.leftNode, value);
        } else if (value > node.value) {
            if (node.rightNode == null) return null;
            else return treeManager.searchDownTree(node.rightNode, value);
        }

        return false;
    },

    deleteNode: (valueOrNode) => {
        let deleteNode;

        if (valueOrNode instanceof treeNode) deleteNode = valueOrNode;
        else deleteNode = treeManager.searchForNode(valueOrNode);

        if (!(deleteNode)) return;

        const children = deleteNode.children();
        const hide = children.length === 2;

        drawSettings.nodesToAnimate.push([{ node: deleteNode, animationType: 'delete', hide }]);

        const childNodesToAnimate = [];
        let replaceNode = null;

        if (children.length === 1) {
            replaceNode = children[0];
            replaceNode.depth = deleteNode.depth;
            replaceNode.locX = deleteNode.locX;
            replaceNode.locY = deleteNode.locY;
            replaceNode.parent = deleteNode.parent;

            treeManager.addToParent(replaceNode);
            childNodesToAnimate.push({
                node: replaceNode,
                animationType: 'move',
                x: deleteNode.locX,
                y: deleteNode.locY,
                drawEdge: true
            });

            $.each(replaceNode.allChildren(), function (index, node) {
                node.depth = node.parent.depth + 1;
                treeManager.addToParent(node);
                childNodesToAnimate.push({
                    node,
                    animationType: 'move',
                    x: node.locX,
                    y: node.locY,
                    lineAnimation: {
                        x1: node.parent.locX - node.locX,
                        y1: node.parent.locY - node.locY, x2: 0, y2: 0
                    }
                });
            });
        } else if (children.length === 2) {
            const findLargestChild = function (node) {
                if (node.rightNode) return findLargestChild(node.rightNode);
                else return node;
            }

            replaceNode = findLargestChild(deleteNode.leftNode);

            drawSettings.nodesToAnimate.push([{
                node: replaceNode,
                animationType: 'move',
                x: deleteNode.locX,
                y: deleteNode.locY,
                removeConnection: true
            }]);

            deleteNode.value = replaceNode.value;

            drawSettings.nodesToAnimate.push([{ node: replaceNode, animationType: 'hide' }]);
            drawSettings.nodesToAnimate.push([{
                node: deleteNode,
                animationType: 'unhide',
                drawEdge: true,
                drawChildrenConnection: true,
                newValue: true
            }]);

            treeManager.deleteNode(replaceNode);
        }

        if (children.length < 2) {
            if (childNodesToAnimate.length > 0) drawSettings.nodesToAnimate.push(childNodesToAnimate);

            if (deleteNode.parent) {
                if (deleteNode.parent.leftNode === deleteNode) {
                    deleteNode.parent.leftNode = replaceNode;
                } else if (deleteNode.parent.rightNode === deleteNode) {
                    deleteNode.parent.rightNode = replaceNode;
                }
            } else {
                rootNode = replaceNode;
                if (replaceNode) replaceNode.parent = null;
            }

            deleteNode = null;
        }
    },

    findParentFromValue: (node, value) => {
        drawSettings.nodesToAnimate.push([{ node, animationType: 'pop' }]);

        if (value < node.value) {
            if (!node.leftNode) return node;
            else return treeManager.findParentFromValue(node.leftNode, value);
        } else {
            if (!node.rightNode) return node;
            else return treeManager.findParentFromValue(node.rightNode, value);
        }
    },

    addNextRandom: (numbersToAdd) => {
        if (numbersToAdd.length > 0) view.addNode(numbersToAdd);
    },

    changeAnimationSpeed: (value, noAnimation) => {
        if (value > 2) drawSettings.animationSpeed.delay = 0;
        else drawSettings.animationSpeed.delay = Math.min(0, -100 * Math.pow(value, 2) + 50 * value + 300);

        drawSettings.animationSpeed.speed = 150 * Math.pow(value, -1);
        drawSettings.animationSpeed.noAnimation = noAnimation;

        if (noAnimation) drawSettings.animationSpeed.speed = 0;

    },

    returnNodesToAnimate: () => {
        return drawSettings.nodesToAnimate;
    },

    returnAnimationSpeed: () => {
        return drawSettings.animationSpeed;
    },

    returnChildren: (node) => {
        return node.children();
    }
};

const view = {
    addNode: function (numbersToAdd) {
        let value;

        if (numbersToAdd) value = numbersToAdd.pop();
        else {
            const input = $('#addNode');
            value = Number(input.val());
            input.val('');
        }

        if (view.checkForBadValue(value)) return;

        treeManager.addNode(value, numbersToAdd);
    },

    draw: function () {
        svg.attr('transform', `translate(${d3.event.translate})scale(${d3.event.scale})`);
    },

    drawNode: function (node) {
        const newNode = svg
            .insert('g', ':first-child')
            .attr({ class: 'node', id: node.id })
            .attr('transform', 'translate(' + node.locX + ',' + node.locY + ')');

        view.drawEdge(node.parent, node);

        const nodeWrap = newNode.append('g').attr({ class: 'nodeWrap', transform: 'scale(0)' });

        nodeWrap.append('circle')
            .attr('r', node.size)
            .style({ 'fill': node.fillStyle, 'stroke': node.fillStyleText, 'stroke-width': 2 });

        nodeWrap.append('text')
            .attr('dx', '0')
            .attr('dy', '.35em')
            .text(node.value);

        return nodeWrap;

    },

    drawEdge: function (node, childNode) {
        if (node) {
            d3.select('#' + childNode.id).insert('line', ':first-child')
                .attr({
                    x1: node.locX - childNode.locX,
                    y1: node.locY - childNode.locY,
                    x2: 0,
                    y2: 0,
                    class: 'nodeLine'
                });
        }

    },

    animateNodes: function (nodesToAnimate, numbersToAdd) {
        let animationSpeed = treeManager.returnAnimationSpeed();
        let speed = animationSpeed.speed;

        const animate = {
            pop: function (animation) {
                if (animationSpeed.noAnimation) endOfTransitions();
                else d3.select('#' + animation.node.id + ' > .nodeWrap')
                    .transition()
                    .attr('transform', 'scale(1.5)')
                    .duration(speed)
                    .transition()
                    .attr('transform', 'scale(1)')
                    .duration(speed)
                    .transition()
                    .attr('transform', null)
                    .duration(0)
                    .each('end', endOfTransitions);
            },

            delete: function (animation) {
                d3.select('#' + animation.node.id + ' > .nodeWrap')
                    .transition()
                    .attr('transform', 'scale(0)')
                    .duration(speed)
                    .each('end', function () {
                        $('#' + animation.node.id + ' > .nodeLine').remove();
                        $.each(treeManager.returnChildren(animation.node), function (index, node) {
                            $('#' + node.id + ' > .nodeLine').remove();
                        })

                        if (!(animation.hide)) $('#' + animation.node.id).remove();

                        endOfTransitions();
                    });
            },

            move: function (animation) {
                if (animation.node.parent) {
                    $('#' + animation.node.id).insertBefore($('#' + animation.node.parent.id));
                } else {
                    $('#' + animation.node.id).appendTo('#svg > g > g');
                }

                if (animation.removeConnection) {
                    $('#' + animation.node.id + ' > .nodeLine').remove();
                    $.each(treeManager.returnChildren(animation.node), function (index, node) {
                        $('#' + node.id + ' > .nodeLine').remove();
                    })
                }

                d3.select('#' + animation.node.id).transition()
                    .attr('transform', 'translate(' + animation.x + ',' + animation.y + ')')
                    .duration(speed)
                    .each('end', function () {
                        if (animation.drawEdge) view.drawEdge(animation.node.parent, animation.node);

                        endOfTransitions();
                    });

                if (animation.lineAnimation) {
                    d3.select('#' + animation.node.id + ' > .nodeLine').transition()
                        .attr({
                            x1: animation.lineAnimation.x1,
                            y1: animation.lineAnimation.y1,
                            x2: animation.lineAnimation.x2,
                            y2: animation.lineAnimation.y2
                        })
                        .duration(speed)
                }
            },

            unhide: function (animation) {
                d3.select('#' + animation.node.id + ' > .nodeWrap').attr('transform', 'scale(1)');

                if (animation.drawEdge) view.drawEdge(animation.node.parent, animation.node);

                if (animation.drawChildrenConnection) {
                    $.each(treeManager.returnChildren(animation.node), function (index, node) {
                        view.drawEdge(node.parent, node);
                    })
                }

                if (animation.newValue) {
                    d3.select('#' + animation.node.id + ' > .nodeWrap > text').text(animation.node.value);
                }

                endOfTransitions();
            },

            hide: function (animation) {
                $('#' + animation.node.id).hide();
                endOfTransitions();
            },

            add: function (animation) {
                const nodeWrap = view.drawNode(animation.node);
                nodeWrap.transition()
                    .attr('transform', 'scale(1)')
                    .duration(speed)
                    .each('end', endOfTransitions);
            }
        }

        function endOfTransitions() {
            nodesToAnimate[0].pop()

            if (nodesToAnimate[0].length === 0) {
                nodesToAnimate.shift();
                animateNextNode();
            }
        }

        function animateAllNodesInSublist(nodesToAnimate) {
            $.each(nodesToAnimate[0], function (index, child) {
                animationSpeed = treeManager.returnAnimationSpeed();
                speed = animationSpeed.speed;
                animate[child.animationType](child);
            });
        }

        function animateNextNode() {
            if (nodesToAnimate.length > 0) animateAllNodesInSublist(nodesToAnimate);
            else {
                setTimeout(function () {
                    if (numbersToAdd && numbersToAdd[0]) {
                        view.addNode(numbersToAdd);
                    }
                }, animationSpeed.delay)
            }
        }

        animateNextNode();
    },

    addRandoms: function () {
        const input = $('#addRandom');
        const value = Number(input.val());
        const numbersToAdd = [];

        input.val('');

        if (view.checkForBadValue(value)) return;


        for (let i = 0; i < value; i++) {
            numbersToAdd.push(Math.floor((Math.random() * 500) + 1));
        }

        treeManager.addNextRandom(numbersToAdd);
    },

    searchForNode: function () {
        const input = $('#searchForNode');
        const value = Number(input.val());

        if (view.checkForBadValue(value)) return;
        if (treeManager.returnAnimationSpeed().noAnimation) return;

        input.val('');

        treeManager.searchForNode(value);
        view.animateNodes(treeManager.returnNodesToAnimate(), null, null);
    },

    deleteNode: function () {
        const input = $('#deleteNode');
        const value = Number(input.val());

        if (view.checkForBadValue(value)) return;

        input.val('');

        treeManager.deleteNode(value);
        view.animateNodes(treeManager.returnNodesToAnimate(), null, null);
    },

    checkForBadValue: function (value) {
        if (isNaN(value)) {
            alert('Введите число!');
            return true;
        }
        return false;
    }
}

$('#animationSpeed').slider({
    formatter: function (value) {
        let noAnimation = false;
        let text = value + 'x';

        if (value > 5) {
            noAnimation = true;
            text = 'No Animation';
        }

        treeManager.changeAnimationSpeed(value, noAnimation);
        return text;
    },
    tooltip: "hide"
});

const zoomHandler = d3.behavior.zoom().scaleExtent([0.5, 10]).on('zoom', view.draw);
const svg = d3.select('#svg')
console.log(d3.behavior.zoom())
    // .call(zoomHandler)
    // .append('g')
    // .append('g');

const formFunctions = {
    addNodeForm: view.addNode,
    addRandomForm: view.addRandoms,
    searchForNodeForm: view.searchForNode,
    deleteNodeForm: view.deleteNode
};

$('.bst-item').on('submit', function (event) {
    event.preventDefault();
    try {
        formFunctions[this.id]();
    } catch (err) {
        console.log(err);
    }
    return false;
});
