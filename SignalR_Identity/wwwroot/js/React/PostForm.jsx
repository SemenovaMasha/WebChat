

 class PostForm extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            userId: props.userId,
            isAuth: props.isAuth,
            addHandle: props.addHandle,
            currentUsername: props.currentUsername,
            text: ''
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.submit = this.submit.bind(this);
    }

    handleChange(event) {
        this.setState({ text: event.target.value });
    }

     handleSubmit(event) {

        if (typeof event !== 'undefined')
            event.preventDefault();

         if (this.state.text !== "") {
             fetch('https://localhost:44358/api/Posts/AddNewPost',
                     {
                         method: 'POST',
                         headers: {
                             'Accept': 'application/json',
                             'Content-Type': 'application/json',
                         },
                         body: JSON.stringify({
                             currentUsername: this.state.currentUsername,
                             text: this.state.text,
                             userId: this.state.userId,
                             userAuth: this.state.isAuth,
                         })
                     })
                 .then(result => result.json())
                 .then(result => {
                     this.state.addHandle(result);
                 });

             this.setState({
                 text: ''
             });
         }
     }

    componentDidMount() {
       
        $("#newPostText").keydown(this.submit).keypress(this.submit);
    }

    submit(evt) {
        if (evt.keyCode == 13 && !evt.shiftKey) {
            this.handleSubmit();
            evt.preventDefault();
        }
    }
    
    render() {
        return (
            <form onSubmit={this.handleSubmit} id="newPostForm">
                <textarea value={this.state.text} onChange={this.handleChange} id="newPostText"/>
                <input type="submit" value="Submit" />
            </form>
        );
    }
}
