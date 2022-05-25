let addEdgeOn = false;
let animationOn = false;
let isRemove = false;
let selectNode;
let selectedEdge;
let graph = new Graph();
let selectedAlgorithm;
const LIST_ALGORITHM = {
    bfs: 'bfs',
    dfs: 'dfs',
    dijkstra: 'dijkstra',
    bellmanFord: 'bellmanFord',
    floydWarshall: 'floydWarshall'
}
const TEXT_HINT = {
    start: 'Выберите начальную вершину',
    end: 'Выберите конечную вершину',
    remove: 'Выберите вершину или ребро, которое хотите удалить',
    weighted: 'Чтобы добавить вес кликните на ребро',
    bfs: ' для обхода в ширину',
    dfs: ' для обхода в глубину',
    dijkstra: ' для алгоритма Дейкстры',
    bellmanFord: ' для алгоритма Беллмана-Форда'
};
const FUNCTION_ALGORITHMS = {bfs, dfs, dijkstra, bellmanFord}
const switchText = {
    directed: 'Ориентированный',
    undirected: 'Неориентированный',
    weighted: 'Взвешенный',
    unweighted: 'Невзвешенный'
}

graph.viewGraph.on('click', function (event) {
    const node = event.target;
    if (node.group) {
        if (addEdgeOn && node.group() === 'nodes') {
            addEdges(node);
        }
        if (isRemove) {
            removeElement(node);
        } else if (graph.isWeighted && node.group() === 'edges') {
            $('.modal').modal('show');
            selectedEdge = node;
        } else if (selectedAlgorithm && node.group() === 'nodes') {
            FUNCTION_ALGORITHMS[selectedAlgorithm](event.target.data().id);
        }
    }
});

/**
 * Метод добавления узла
 */
addNode.addEventListener('click', () => {
    resetAnimation();
    $('#switchOriented').prop('disabled', true);
    $('#switchWeighted').prop('disabled', true);
    graph.addNode();
});

/**
 * Метод добавления ребра
 */
addEdge.addEventListener('click', () => {
    resetAnimation();
    $('#hint').text(TEXT_HINT.start);
    addEdgeOn = true;
});

/**
 * Обработчик клика по меню с алгоритмами
 */
$('.dropdown-item').on('click', (event) => {
    resetAnimation();
    if (event.target.id === 'floydWarshall') {
        floydWarshall();
    } else if (event.target.id === 'prima') {
        prima();
    } else {
        selectedAlgorithm = LIST_ALGORITHM[event.target.id];
        $('#hint').text(TEXT_HINT.start + TEXT_HINT[event.target.id]);
    }
});

/**
 * Перезагрузка страницы
 */
$('#resetBtn').on('click', () => {
    location.reload();
});

$('#createRandomGraph').on('click', () => {
    graph.createGraph($('#addRandom').val());
    $('#switchOriented').prop('disabled', true);
    $('#switchWeighted').prop('disabled', true);
});

/**
 * Удаление элемента графа
 */
$('#removeGraphElement').on('click', () => {
    resetAnimation();
    isRemove = !isRemove;
    $('#hint').text(isRemove ? TEXT_HINT.remove : '');
})

/**
 * Добавление веса
 */
$('#addWeight').on('click', () => {
    const weight = $('#edge-weight').val();
    graph.addWeight(selectedEdge, weight);
    const domEdge = $(`.${selectedEdge.data('id')}`)[0];
    if (domEdge) {
        domEdge.textContent = weight;
    } else {
        setPopperElement(selectedEdge, weight);
    }
    $('#edge-weight').val(1);
    $('.modal').modal('hide');
})

$('#edge-weight').on('input', () => {
    $('#addWeight').prop('disabled', !$('#edge-weight').val());
})

/**
 * Обработка изменения значения у switch
 */
$('.checkbox').on('change', (event) => {
    const idChange = event.target.id;
    if (idChange === 'switchOriented') {
        $(`[for="${event.target.id}"]`).text(event.target.checked ? switchText.directed : switchText.undirected);
        graph.isOriented = !!event.target.checked;
        graph.viewGraph.style().selector('edge').style({'target-arrow-shape': event.target.checked ? 'triangle': 'none'})
    } else {
        $(`[for="${event.target.id}"]`).text(event.target.checked ? switchText.weighted : switchText.unweighted);
        graph.isWeighted = !!event.target.checked;
        hintForWeight
        $('#hintForWeight').text(graph.isWeighted ? TEXT_HINT.weighted : '');
    }
});

$('#addRandom').on('input', () => {
    $('#createRandomGraph').prop('disabled', !$('#addRandom').val());
});

function animateBFS(elements) {
    animationOn = true;
    elements.forEach((element, index) => {
        setTimeout(function () {
            element.animate({
                style: {'background-color': '#b2cfff'}
            }, {
                duration: 500
            });
        }, 500 * (index + 1))
    });
}

function animateDijkstra(elements) {
    animationOn = true;
    let sumLength = [];
    elements.forEach((element, index) => {
        sumLength[index] = element.path.length + (index > 0 ? sumLength[index - 1] : 0)
    });
    elements.forEach((element, index) => {
        setTimeout(function() {
            if (element.path.length) {
                element.start.animate({
                    style: {'background-color': '#b7ffb2'}
                }, {
                    duration: 300
                });
            }
            if (index > 0) {
                elements[index-1].start.animate({
                    style: {'background-color': '#ffb2b2'}
                }, {
                    duration: 300
                });
            }
            element.path.forEach((pathEl, indexEl) => {
                setTimeout(function () {
                    pathEl.end.animate({
                        style: {'background-color': '#b2cfff'}
                    }, {
                        duration: 500
                    });
                    if (pathEl.weight) {
                        const domEdge = $(`.${pathEl.end.data('id')}`)[0];
                        if (domEdge) {
                            domEdge.textContent = pathEl.weight;
                        } else {
                            setPopperElement(pathEl.end, pathEl.weight);
                        }
                    }
                }, 600 * (indexEl + 1));
            });

        }, 1200 * (index > 0 ? sumLength[index - 1] + 1 : 1))
    });
}

function animationBellmanFord(path) {
    animationOn = true;
    path.forEach((element, index) => {
        setTimeout(function () {
            element.start.animate({
                style: {'background-color': '#b2cfff'}
            }, {
                duration: 300
            });
            element.end.delay(300).animate({
                style: {'background-color': '#b2cfff'}
            }, {
                duration: 300
            });

            setTimeout(function() {
                const domEdge = $(`.${element.end.data('id')}`)[0];
                if (domEdge) {
                    domEdge.textContent = element.distance;
                } else {
                    setPopperElement(element.end, element.distance);
                }
            }, 800);
        }, 1500 * (index + 1))
    });
}

function animationPrima(path) {
    animationOn = true;
    path.forEach((element, index) => {
        setTimeout(function () {
            const edge = graph.viewGraph.edges().find(edge =>
                Number(edge.data().source) === Number(element.data().id) && Number(edge.data().target) === index ||
                Number(edge.data().target) === Number(element.data().id) && Number(edge.data().source)=== index)
            element.animate({
                style: {'background-color': '#b2cfff'}
            }, {
                duration: 300
            });
            if (edge) {
                edge.delay(300).animate({
                    style: {
                        'line-color': '#b2cfff',
                        'target-arrow-color': '#b2cfff',}
                }, {
                    duration: 300
                });
                graph.viewGraph.elements(`node#${index}`).delay(500).animate({
                    style: {'background-color': '#b2cfff'}
                }, {
                    duration: 300
                });
            }

        }, 1500 * (index + 1))
    });
}

function setPopperElement(element, textContent) {
    const makeDiv = function(text) {
        const div = document.createElement('div');
        div.classList.add(`${element.data('id')}`);
        if (element.group() !== 'edges') {
            div.classList.add('popper-div');
        }
        div.innerHTML = text;
        document.body.appendChild(div);
        return div;
    };
    const popperElement = element.popper({
        content: function(){ return makeDiv(textContent); }
    });
    const update = function() {
        popperElement.update();
    }
    if (element.group() === 'edges') {
        graph.viewGraph.elements(`node#${element.data('source')}`).on('position', update);
        graph.viewGraph.elements(`node#${element.data('target')}`).on('position', update);
    }
    element.on('position', update);
    graph.viewGraph.on('pan resize', update);
}

function resetAnimation() {
    if (animationOn) {
        graph.viewGraph.nodes().forEach(node => {
            node.style({'background-color': '#ccc'});
        });
        graph.viewGraph.edges().forEach(edge => {
            edge.style({
                'line-color': '#6d6d6d',
                'target-arrow-color': '#6d6d6d'});
        });
        const poppers = document.querySelectorAll('.popper-div');
        poppers.forEach(popper => {
            popper.remove();
        });
        const thead = document.querySelector('.theadForAlgo');
        while (thead?.firstChild) {
            thead.removeChild(thead.firstChild);
        }
        const tbody = document.querySelector('.tbodyForAlgo');
        while (tbody?.firstChild) {
            tbody.removeChild(tbody.firstChild);
        }
        animationOn = false;
    }
    $('table').addClass('hidden');
}

function addEdges(node) {
    if (selectNode) {
        $('#hint').text('');
        const newEdge = graph.addEdge(selectNode, node);
        node.unselect();
        addEdgeOn = false;
        selectNode = null;
        if (graph.isWeighted) {
            setPopperElement(newEdge, '1');
        }
    } else {
        selectNode = node;
        $('#hint').text(TEXT_HINT.end);
    }
}
function removeElement(element) {
    isRemove = false;
    graph.removeElement(element);
    $('#switchOriented').prop('disabled', !!graph.nodes.length);
    $('#switchWeighted').prop('disabled', !!graph.nodes.length);
    $('#hint').text('');
}

function bfs(start) {
    const path = graph.bfs(start);
    animateBFS(path);
    resetSelectedAlgo();
}

function dfs(start) {
    const path = graph.dfs(start);
    animateBFS(path);
    resetSelectedAlgo();
}
function dijkstra(start) {
    const path = graph.dijkstra(start);
    animateDijkstra(path);
    resetSelectedAlgo();
}

function bellmanFord(start) {
    const path = graph.bellmanFord(start);
    animationBellmanFord(path);
    resetSelectedAlgo();
}

function floydWarshall() {
    animationOn = true;
    const matrix = graph.floydWarshall();
    const trHead = document.createElement('tr');
    const thHead = document.createElement('th');
    trHead.appendChild(thHead);
    matrix.forEach((row, indexRow) => {
        const thHead = document.createElement('th');
        const thBody = document.createElement('th');
        const tr = document.createElement('tr');
        thHead.textContent = String(indexRow);
        trHead.appendChild(thHead);
        thBody.textContent = String(indexRow);
        tr.appendChild(thBody);
        row.forEach(col => {
            const td = document.createElement('td');
            td.textContent = col;
            tr.appendChild(td);
        });
        $('tbody').append(tr);
    });
    $('thead').append(trHead);
    $('.tableForGraph').removeClass('hidden');
}

function prima() {
    const result = graph.prima();
    const path = [];
    result.forEach((item, index) => {
        path[index] = item === -1 ? graph.viewGraph.elements(`node#${0}`) : graph.viewGraph.elements(`node#${item}`);
    });
    animationPrima(path);
}

function resetSelectedAlgo() {
    selectedAlgorithm = null;
    $('#hint').text('');
}