 class PostsList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            userId: props.userId,
            posts: []
        }
    }

     addNewPost(post) {
         console.log(post);
        this.state.posts.unshift(post);
        this.setState({ state: this.state });
    }

     render() {
        // if (typeof (this.state.posts) !== "undefined" && this.state.posts !== null) { 

        //console.log(typeof(this.state.posts));
        //     console.log(this.state.posts[0]);

        //     if (typeof (this.state.posts[0]) !== "undefined" && this.state.posts[0] !== null) {
        //         console.log(Object.keys(this.state.posts[0]));
        //         console.log(this.state.posts[0].userAuth);
        //     }
        // }

        const posts = this.state.posts.map(post => (
            <Post id={post.id}
                text={post.text}
                date={post.date}
                liked={post.liked}
                userAuth={post.userAuth}
                authorName={post.authorName}
                authorAvatarPath={post.authorAvatarPath}
                key={post.id}
            >
            </Post>
        ));

        return <div id="feedList">
                   <ul className="list-group">
                       {posts}
                   </ul>
               </div>;

    }

    componentWillMount() {
        this.getItems();
    }


    getItems() {
        fetch('https://localhost:44358/api/Posts/GetPostsByUserId/' + this.state.userId)
            .then(results => results.json())
            .then(results => {
                this.setState({ posts: results });
            });
    }
}
