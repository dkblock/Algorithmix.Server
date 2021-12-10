const rotateLeft = (node, fn) => {
  const parent = node.parent;
  const right = node.children[1];

  if (right.children.length === 0) {
    right.children.push({ id: id++, data: null, parent: right, children: [] });
    right.children.push({ id: id++, data: null, parent: right, children: [] });
  }

  if (parent === null) {
    right.children[0].parent = node;
    node.children[1] = right.children[0];

    node.parent = right;
    right.children[0] = node;

    right.parent = parent;
    data = right;

    gLinks.selectAll('path').filter((d) => d.data.id === node.parent.id).datum(d3.hierarchy(node).descendants()[0]);
    rootChanged = true;
  } else if (node === parent.children[0]) {
    right.children[0].parent = node;
    node.children[1] = right.children[0];

    node.parent = right;
    right.children[0] = node;

    right.parent = parent;
    parent.children[0] = right;

    rootChanged = false;
  } else if (node === parent.children[1]) {
    right.children[0].parent = node;
    node.children[1] = right.children[0];

    node.parent = right;
    right.children[0] = node;

    right.parent = parent;
    parent.children[1] = right;

    rootChanged = false;
  }

  if (node.children.length !== 0 && node.children[0].data === null && node.children[1].data === null) {
    node.children = [];
  }

  setTimeout(function () {
    if (fn instanceof Function) {
      fn();
    }
  }, duration);
};

const rotateRight = (node, fn) => {
  const parent = node.parent;
  const left = node.children[0];

  if (left.children.length === 0) {
    left.children.push({ id: id++, data: null, parent: left, children: [] });
    left.children.push({ id: id++, data: null, parent: left, children: [] });
  }

  if (parent === null) {
    left.children[1].parent = node;
    node.children[0] = left.children[1];

    node.parent = left;
    left.children[1] = node;

    left.parent = parent;
    data = left;

    gLinks.selectAll('path').filter((d) => d.data.id === node.parent.id).datum(d3.hierarchy(node).descendants()[0]);
    rootChanged = true;
  } else if (node === parent.children[0]) {
    left.children[1].parent = node;
    node.children[0] = left.children[1];

    node.parent = left;
    left.children[1] = node;

    left.parent = parent;
    parent.children[0] = left;

    rootChanged = false;
  } else if (node === parent.children[1]) {
    left.children[1].parent = node;
    node.children[0] = left.children[1];

    node.parent = left;
    left.children[1] = node;

    left.parent = parent;
    parent.children[1] = left;

    rootChanged = false;
  }

  if (node.children.length !== 0 && node.children[0].data === null && node.children[1].data === null) {
    node.children = [];
  }

  setTimeout(function () {
    if (fn instanceof Function) {
      fn();
    }
  }, duration);
};