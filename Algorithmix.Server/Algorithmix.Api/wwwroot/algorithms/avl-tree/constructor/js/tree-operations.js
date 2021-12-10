let id = 1;
let data = {
  id: id++,
  data: null,
  parent: null,
  children: []
};

const insertNode = (number, fn) => {
  if (!number || !Number.isInteger(number)) return;
  if (!data.data) {
    data.data = number;
    updateTree();
    fn();
    return;
  }

  let current = data;
  let newNode;

  while (!newNode) {
    if (number <= current.data) {
      if (current.children.length === 0) {
        current.children.push({ id: id++, data: number, parent: current, children: [] });
        current.children.push({ id: id++, data: null, parent: current, children: [] });
        newNode = current.children[0];
      } else if (current.children[0].data === null) {
        current.children[0].data = number;
        newNode = current.children[0];
      } else {
        current = current.children[0];
      }
    } else {
      if (current.children.length === 0) {
        current.children.push({ id: id++, data: null, parent: current, children: [] });
        current.children.push({ id: id++, data: number, parent: current, children: [] });
        newNode = current.children[1];
      } else if (current.children[1].data === null) {
        current.children[1].data = number;
        newNode = current.children[1];
      } else {
        current = current.children[1];
      }
    }
  }

  updateTree();
  setTimeout(function () {
    balanceTree(newNode, fn);
  }, duration);
};

const deleteNode = (number, fn) => {
  if (!data.data) {
    return false;
  }

  let current = data;
  let nodeToDelete = null
  let nodeToReplace = null;
  let nodeToBalance = null
  let parent;

  if (number === current.data) {
    nodeToDelete = current;
    if (nodeToDelete.children.length === 0) {
      nodeToDelete.data = null;
    } else {
      if (nodeToDelete.children[0].data === null) {
        data = data.children[1];
        data.parent = null;
        gLinks.selectAll('path').filter((d) => d.data.id === data.id).remove();
      } else {
        nodeToReplace = nodeToDelete.children[0];

        while (nodeToReplace) {
          if (!nodeToReplace.children[1] || !nodeToReplace.children[1].data) {
            break;
          }

          nodeToReplace = nodeToReplace.children[1];
        }

        parent = nodeToReplace.parent;
        nodeToBalance = parent;

        if (parent.children[0] === nodeToReplace) {
          if (nodeToReplace.children[0]) {
            nodeToReplace.children[0].parent = parent;
            parent.children[0] = nodeToReplace.children[0]
          } else {
            parent.children[0] = { id: id++, data: null, parent: parent, children: [] };
          }
        } else if (parent.children[1] === nodeToReplace) {
          if (nodeToReplace.children[0]) {
            nodeToReplace.children[0].parent = parent;
            parent.children[1] = nodeToReplace.children[0]
          } else {
            parent.children[1] = { id: id++, data: null, parent: parent, children: [] };
          }
        }

        if (parent.children.length !== 0 && parent.children[0].data === null && parent.children[1].data === null) {
          parent.children = [];
        }

        if (nodeToDelete.children[0]) {
          nodeToDelete.children[0].parent = nodeToReplace;
          nodeToReplace.children[0] = nodeToDelete.children[0];
        }
        if (nodeToDelete.children[1]) {
          nodeToDelete.children[1].parent = nodeToReplace;
          nodeToReplace.children[1] = nodeToDelete.children[1];
        }

        nodeToReplace.parent = null;
        data = nodeToReplace;
        gLinks.selectAll('path').filter((d) => d.data.id === nodeToReplace.id).remove();
      }
    }

    updateTree();
    setTimeout(() => {
      if (nodeToBalance) {
        balanceTree(nodeToBalance, fn);
      } else if (fn instanceof Function) {
        fn();
      }
    }, duration);

    return true;
  }

  while (current.data) {
    if (number < current.data) {
      current = current.children[0];
      if (!current) {
        break;
      }
    } else if (number > current.data) {
      current = current.children[1];
      if (!current) {
        break;
      }
    } else if (number === current.data) {
      nodeToDelete = current;
      break;
    }
  }

  if (!nodeToDelete) {
    alert('Значение ' + number + ' отсутствует в дереве.');
    return true;
  }

  if (nodeToDelete.children.length === 0) {
    parent = nodeToDelete.parent;
    nodeToBalance = parent;

    if (parent.children[0] === nodeToDelete) {
      parent.children[0] = { id: id++, data: null, parent: parent, children: [] };
    } else if (parent.children[1] === nodeToDelete) { // Remove right child
      parent.children[1] = { id: id++, data: null, parent: parent, children: [] };
    }

    if (parent.children.length !== 0 && parent.children[0].data === null && parent.children[1].data === null) {
      parent.children = [];
    }
  } else {
    nodeToReplace = nodeToDelete.children[0].data ? nodeToDelete.children[0] : null;

    while (nodeToReplace) {
      if (!nodeToReplace.children[1] || !nodeToReplace.children[1].data) {
        break;
      }
      nodeToReplace = nodeToReplace.children[1];
    }

    if (!nodeToReplace) {
      parent = nodeToDelete.parent;
      nodeToBalance = parent;
      nodeToDelete.children[1].parent = parent;

      if (parent.children[0] === nodeToDelete) {
        parent.children[0] = nodeToDelete.children[1];
      } else if (parent.children[1] === nodeToDelete) {
        parent.children[1] = nodeToDelete.children[1];
      }
    } else {
      parent = nodeToReplace.parent;
      nodeToBalance = parent;

      if (parent.children[0] === nodeToReplace) {
        if (nodeToReplace.children[0]) {
          nodeToReplace.children[0].parent = parent;
          parent.children[0] = nodeToReplace.children[0]
        } else {
          parent.children[0] = { id: id++, data: null, parent: parent, children: [] };
        }
      } else if (parent.children[1] === nodeToReplace) {
        if (nodeToReplace.children[0]) {
          nodeToReplace.children[0].parent = parent;
          parent.children[1] = nodeToReplace.children[0]
        } else {
          parent.children[1] = { id: id++, data: null, parent: parent, children: [] };
        }
      }

      if (parent.children.length !== 0 && parent.children[0].data === null && parent.children[1].data === null) {
        parent.children = [];
      }

      parent = nodeToDelete.parent;
      nodeToReplace.parent = parent;
      if (parent.children[0] === nodeToDelete) parent.children[0] = nodeToReplace;
      else if (parent.children[1] === nodeToDelete) parent.children[1] = nodeToReplace;

      if (nodeToDelete.children[0]) {
        nodeToDelete.children[0].parent = nodeToReplace;
        nodeToReplace.children[0] = nodeToDelete.children[0];
      }
      if (nodeToDelete.children[1]) {
        nodeToDelete.children[1].parent = nodeToReplace;
        nodeToReplace.children[1] = nodeToDelete.children[1];
      }
    }
  }

  updateTree();
  setTimeout(() => {
    if (nodeToBalance) {
      balanceTree(nodeToBalance, fn);
    } else if (fn instanceof Function) {
      fn();
    }
  }, duration);
  return true;
};

const searchNode = (number) => {
  const searchDownTree = (node, fn) => {
    if (!node) {
      fn(false);
      return;
    }

    highlightNode(node);
    setTimeout(() => {
      removeHighlightFromNode(node);

      if (number < node.data) {
        searchDownTree(node.children[0], fn);
      } else if (number > node.data) {
        searchDownTree(node.children[1], fn);
      } else {
        fn(true);
      }
    }, duration);
  }

  searchDownTree(data, (exist) => {
    if (exist) {
      alert('Значение ' + number + ' найдено');
    } else {
      alert('Значение ' + number + ' отсутствует в дереве');
    }
  });
}

const fillTree = (count) => {
  const generateNumbers = (count) => {
    const numbers = [];

    for (let i = 0; i < count; i++) {
      const generated = Math.floor(Math.random() * 100);
      numbers.push(generated);
    }

    return numbers;
  }

  const generateFns = (count) => {
    const fns = [];

    for (let i = 0; i < count; i++) {
      const fn = () => insertNode(numbers[i], () => {
        setTimeout(() => {
          if (fns[i + 1]) {
            fns[i + 1]();
          }
        }, duration);
      });

      fns.push(fn);
    }

    return fns;
  }


  const numbers = generateNumbers(count);
  const fns = generateFns(count);

  insertNode(numbers[0], () => {
    setTimeout(() => {
      fns[1]();
    }, duration);
  });
}

const getHeight = (node) => {
  if (node.data === null || !node) return 0;
  else {
    const left = node.children[0] ? getHeight(node.children[0]) : 0;
    const right = node.children[1] ? getHeight(node.children[1]) : 0;

    return 1 + ((left > right) ? left : right);
  }
};

const balanceTree = (node, fn) => {
  highlightNode(node);

  const leftHeight = node.children[0] ? getHeight(node.children[0]) : 0;
  const rightHeight = node.children[1] ? getHeight(node.children[1]) : 0;
  let hl, hr, delta = 0.5;

  if (leftHeight - rightHeight >= 2) {
    const left = node.children[0];
    hl = left.children[0] ? getHeight(left.children[0]) : 0;
    hr = left.children[1] ? getHeight(left.children[1]) : 0;

    if (hl >= hr) {
      rotateRight(node, updateTree);
      delta = 1;
    } else {
      delta = 3;
      rotateLeft(left, () => {
        updateTree();
        setTimeout(() => {
          rotateRight(node, updateTree);
        }, duration);
      });
    }
  } else if (rightHeight - leftHeight >= 2) {
    const right = node.children[1];
    hl = right.children[0] ? getHeight(right.children[0]) : 0;
    hr = right.children[1] ? getHeight(right.children[1]) : 0;

    if (hr >= hl) {
      rotateLeft(node, updateTree);
      delta = 1;
    } else {
      delta = 3;
      rotateRight(right, () => {
        updateTree();
        setTimeout(() => {
          rotateLeft(node, updateTree);
        }, duration);
      });
    }
  }

  setTimeout(() => {
    removeHighlightFromNode(node);
    if (!node.parent) {
      if (fn instanceof Function) fn();
    } else balanceTree(node.parent, fn);
  }, duration * delta);
};

const highlightNode = (node) => {
  const hlNode = gNodes.selectAll('circle').filter((d) => d.data.id === node.id);
  hlNode.transition().duration(duration / 3).attr('transform', 'scale(1.5)');
};

const removeHighlightFromNode = (node) => {
  const hlNode = gNodes.selectAll('circle').filter((d) => d.data.id === node.id);
  hlNode.transition().duration(duration / 3).attr('transform', 'scale(1)');
};